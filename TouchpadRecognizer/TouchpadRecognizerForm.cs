using System.Reflection;
using System.Runtime.InteropServices;

namespace TouchpadRecognizer;

// ウィンドウメッセージを受け取るためだけにメモリ上に生成するフォーム。
public class TouchpadRecognizerForm : Form
{
    private uint? _currentScanTime = null;
    private readonly List<TouchpadContact> _currentFrameContacts = new();
    private readonly Dictionary<int, TouchpadContactState> _states = new();

    // タッチパッドから指が全て離された場合、WM_INPUTメッセージが受け取れなくなり、タッチパッドの状態を把握できなくなる。
    // そのため、WM_INPUTメッセージが一定時間途切れたら指が全て離されたとみなす。
    private readonly System.Windows.Forms.Timer _inactivityTimer = new();

    // 確実にリソースを解放するために参照できるようにしておく。
    private readonly NotifyIcon? _nIcon;
    private readonly ContextMenuStrip? _menu;

    public TouchpadRecognizerForm()
    {
        RegisterTouchpad(false);

        _inactivityTimer.Tick += OnAllFingersLifted;
        _inactivityTimer.Stop();

        // タスクトレイに表示するアイコンの設定
        // アイコンから設定の変更やアプリ終了を行えるようにする
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("TouchpadRecognizer.img.Icon.ico");
        var icon = (stream is null) ? SystemIcons.Application : new Icon(stream);
        _menu = new ContextMenuStrip();
        var confirmItem = new ToolStripMenuItem("入力されるキーの確認");
        confirmItem.Click += (s, e) => {
            using var confirmForm = new KeyConfirmForm
            {
                Icon = icon
            };
            confirmForm.ShowDialog();
        };
        var settingItem = new ToolStripMenuItem("ユーザー設定");
        settingItem.Click += (s, e) => {
            using var settingForm = new UserSettingsForm
            {
                Icon = icon
            };
            settingForm.ShowDialog();
        };
        var exitItem = new ToolStripMenuItem("終了");
        exitItem.Click += (s, e) => {
            Application.Exit();
        };
        _menu.Items.Add(confirmItem);
        _menu.Items.Add(settingItem);
        _menu.Items.Add(exitItem);
        _nIcon = new NotifyIcon
        {
            Icon = icon,
            Text = "TouchpadRecognizer",
            Visible = true,
            ContextMenuStrip = _menu
        };
    }

    public void RegisterTouchpad(bool remove)
    {
        var device = new RAWINPUTDEVICE(0x0D/* Digitizer */, 0x05/* Touchpad */, (remove) ? Null<HWND>.Value : (HWND)this.Handle, remove);
        _ = RegisterRawInputDevices(in device, 1, Marshal.SizeOf<RAWINPUTDEVICE>());
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _inactivityTimer.Stop();
        _inactivityTimer.Tick -= OnAllFingersLifted;
        _nIcon?.Visible = false;
        _nIcon?.Dispose();
        _menu?.Dispose();
        RegisterTouchpad(true);
        base.OnFormClosing(e);
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x00FF/* WM_INPUT */)
        {
            // インターバルは接触開始時にのみ変更する（毎フレーム呼び出す処理ではないため）。
            if (!_inactivityTimer.Enabled)
            {
                // 実行中に変更されたユーザー設定を反映する
                _inactivityTimer.Interval = UserSettings.Instance.InactivityTimeoutMs;
            }
            // 指が触れている間はタイマーを延長
            ResetInactivityTimer();

            var (scanTime, contacts) = TouchpadHelper.ParseInputWithScanTime(m.LParam);

            if (contacts is null)
            {
                base.WndProc(ref m);
                return;
            }

            //   並列モードの場合、本当なら1回目の呼び出し時点でコンタクトグループを確定できる（＝ProcessFrame()を呼べる）が、
            //   コンタクトが1つの場合はレポート数がハイブリッドモードと同じになり、区別できないため、ハイブリッドモードと同様、次のフレームで確定させる。
            if (_currentScanTime is null)
            {
                _currentScanTime = scanTime;
                _currentFrameContacts.AddRange(contacts);
            }
            else if (_currentScanTime == scanTime)
            {
                // 同じScanTimeのコンタクトは同フレームとみなす
                _currentFrameContacts.AddRange(contacts);
            }
            else
            {
                // ScanTimeが変化したらフレームの区切りとみなし、直前のフレームのコンタクトグループを確定させる
                ProcessFrame(_currentFrameContacts.ToArray());

                // 新しいフレームの開始
                _currentFrameContacts.Clear();
                _currentFrameContacts.AddRange(contacts);
                _currentScanTime = scanTime;
            }
        }
        base.WndProc(ref m);
    }
    void ResetInactivityTimer()
    {
        _inactivityTimer.Stop();
        _inactivityTimer.Start();
    }

    private void OnAllFingersLifted(object? sender, EventArgs e)
    {
        _inactivityTimer.Stop();

        var removedIds = _states.Keys.ToList();
        if (removedIds.Count == 0) return;

        if (IsTwoFingerTap(removedIds))
        {
            OnTwoFingerTap(removedIds);
        }
        
        // 保存していた状態を全てリセット
        _states.Clear();
        _currentFrameContacts.Clear();
        _currentScanTime = null;
    }

    private void ProcessFrame(TouchpadContact[] contacts)
    {
        // 現在時刻
        var now = DateTime.UtcNow;
        // 現在接触しているIDのリスト
        var incomingIds = contacts.Select(c => c.ContactId).ToList();

        Dictionary<int, TouchpadContactState> newStates = new();
        foreach (var c in contacts)
        {
            if (!_states.TryGetValue(c.ContactId, out var st))
            {
                // 新しい接触
                var newState = new TouchpadContactState(c.ContactId, c.X, c.Y, now);
                newStates[c.ContactId] = newState;
            }
            else
            {
                // 既存接触の更新
                st.OnUpdate(c.X, c.Y, now);
            }
        }
        // 新しい接触が追加された場合、既存の接触はタップ判定にはならない
        if (newStates.Count > 0)
        {
            foreach (var kv in _states)
            {
                kv.Value.OnOtherContactAdded(now);
            }

            // 新しい接触状態をstatesに追加
            foreach (var kv in newStates)
            {
                _states[kv.Key] = kv.Value;
            }
        }

        // 離されている指があったら、タップ判定を行う
        var removedIds = _states.Keys.Except(incomingIds).ToList();
        if (removedIds.Count == 0) return;
        
        if (IsTwoFingerTap(removedIds))
        {
            OnTwoFingerTap(removedIds);
        }
        foreach (var id in removedIds)
        {
            _states.Remove(id);
        }
    }

    // タップ判定中の指を全てチェックし、「それらの指”だけ”が”全て”離された場合のみ」タップとみなす。
    // 例えば、2本の指がタップ判定中の時は、2本指タップの判定のみ行い、2本中1本だけ離された場合でも、1本指タップとはみなさない。
    // また、2本の指が触れている状態で1本の指のみがタップ判定中の場合、同時に2本とも指を離しても1本指タップとはみなさない。
    private bool IsTwoFingerTap(List<int> removedIds)
    {
        var judgingTapIds = _states.Where(kv => kv.Value.IsJudgingTap).Select(kv => kv.Key).ToList();
        return removedIds.Count == 2 && judgingTapIds.Count == 2 && !removedIds.Except(judgingTapIds).Any();
    }

    private void OnTwoFingerTap(List<int> removedIds)
    {
        var con1 = (X: _states[removedIds[0]].LastX, Y: _states[removedIds[0]].LastY);
        var con2 = (X: _states[removedIds[1]].LastX, Y: _states[removedIds[1]].LastY);
        var pos = TouchpadHelper.JudgeTouchPos(con1, con2);
        if (pos is null) return;

        SendFKey((TouchpadHelper.TouchPos)pos);
    }

    private class TouchpadContactState
    {
        public int ContactId { get; }
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int LastX { get; private set; }
        public int LastY { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime LastUpdateTime { get; private set; }
        public bool IsJudgingTap { get; private set; }

        public TouchpadContactState(int contactId, int startX, int startY, DateTime now)
        {
            ContactId = contactId;
            StartX = startX;
            StartY = startY;
            LastX = startX;
            LastY = startY;
            StartTime = now;
            LastUpdateTime = now;
            IsJudgingTap = true;
        }

        public void OnUpdate(int x, int y, DateTime now)
        {
            LastX = x;
            LastY = y;
            LastUpdateTime = now;

            if (!IsJudgingTap) return;

            // 移動してから戻って来る場合を考慮し、規定移動距離を超えた時点でタップを無効にする
            var dx = LastX - StartX;
            var dy = LastY - StartY;
            var d2 = (double)dx * dx + (double)dy * dy;
            if (Math.Sqrt(d2) > UserSettings.Instance.TapDistanceThresholdPx)
            {
                IsJudgingTap = false;
                return;
            }

            var durationMs = (now - StartTime).TotalMilliseconds;
            if (durationMs > UserSettings.Instance.TapTimeThresholdMs)
            {
                IsJudgingTap = false;
            }
        }

        // 新しい接触とこの接触の開始時間が許容時間内なら、それらを同時押しとみなす
        public void OnOtherContactAdded(DateTime now)
        {
            var durationMs = (now - StartTime).TotalMilliseconds;
            if (durationMs > UserSettings.Instance.AcceptableDelayMs)
            {
                IsJudgingTap = false;
            }
        }
    }

    #region RegisterRawInputDevices
    [Flags]
    enum RIDEV : int
    {
        // 受信を停止する
        RIDEV_REMOVE = 0x00000001,
        // レガシメッセージを生成しない
        RIDEV_NOLEGACY = 0x00000030,
        // 非フォアグラウンドでも入力を受け取る
        RIDEV_INPUTSINK = 0x00000100,
    }
    interface IPtrHandle : IEquatable<IntPtr>
    {
        IntPtr Value { get; set; }
        bool IEquatable<IntPtr>.Equals(IntPtr other) => Value != other;
    }
    static class Null<T> where T : unmanaged, IPtrHandle
    {
        public static T Value = new T() { Value = IntPtr.Zero };
    }
    record struct HWND(IntPtr Value) : IPtrHandle
    {
        public static explicit operator IntPtr(HWND value) => value.Value;
        public static explicit operator HWND(IntPtr value) => new HWND(value);
    }
    struct RAWINPUTDEVICE
    {
        public ushort UsagePage;
        public ushort Usage;
        public RIDEV Flags;
        public HWND HWndTarget;
        public RAWINPUTDEVICE(ushort usagePage, ushort usage, HWND hWndTarget, bool remove)
        {
            UsagePage = usagePage;
            Usage = usage;

            if (remove)
            {
                Flags = RIDEV.RIDEV_REMOVE;
                HWndTarget = Null<HWND>.Value;
            }
            else
            {
                Flags = RIDEV.RIDEV_INPUTSINK;
                HWndTarget = hWndTarget;
            }
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern int RegisterRawInputDevices(
        in RAWINPUTDEVICE devices,
        int number,
        int size);
    #endregion

    #region SendInput
    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public int type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int X;
        public int Y;
        public int Data;
        public int Flags;
        public int Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public short wVk;
        public short wScan;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern uint SendInput(int nInputs, INPUT[] pInputs, int cbSize);
    #endregion

    private enum FKey : short
    {
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87
    }

    private void SendFKey(TouchpadHelper.TouchPos tp)
    {
        INPUT[] inputs = tp switch
        {
            TouchpadHelper.TouchPos.UL => CreateInputArray((short)FKey.F14),
            TouchpadHelper.TouchPos.UR => CreateInputArray((short)FKey.F15),
            TouchpadHelper.TouchPos.LL => CreateInputArray((short)FKey.F16),
            TouchpadHelper.TouchPos.LR => CreateInputArray((short)FKey.F17),
            TouchpadHelper.TouchPos.UL_UR => CreateInputArray((short)FKey.F18),
            TouchpadHelper.TouchPos.UL_LL => CreateInputArray((short)FKey.F19),
            TouchpadHelper.TouchPos.UL_LR => CreateInputArray((short)FKey.F20),
            TouchpadHelper.TouchPos.UR_LL => CreateInputArray((short)FKey.F21),
            TouchpadHelper.TouchPos.UR_LR => CreateInputArray((short)FKey.F22),
            TouchpadHelper.TouchPos.LL_LR => CreateInputArray((short)FKey.F23),
            _ => CreateInputArray((short)FKey.F13),
        };
        var sent = SendInput(inputs.Length, inputs, Marshal.SizeOf<INPUT>());
        if (sent != inputs.Length)
        {
            Console.WriteLine($"SendInput failed: {Marshal.GetLastWin32Error()}");
        }
    }

    private INPUT[] CreateInputArray(short fKeyCode)
    {
        const int INPUT_KEYBOARD = 1;
        const int KEYEVENTF_KEYUP = 0x0002;

        var down = new INPUT
        {
            type = INPUT_KEYBOARD,
            U = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = fKeyCode,
                    wScan = 0,
                    dwFlags = 0,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };

        var up = new INPUT
        {
            type = INPUT_KEYBOARD,
            U = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = fKeyCode,
                    wScan = 0,
                    dwFlags = KEYEVENTF_KEYUP,
                    time = 0,
                    dwExtraInfo = IntPtr.Zero
                }
            }
        };

        return [down, up];
    }
}