using System.Data;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace TouchpadRecognizer
{
    public partial class KeyConfirmForm : Form
    {
        private FileStream? _fileStream;
        private string _ahkFileContent = string.Empty;
        private Encoding? _currentEncoding;

        // 最後にテキストが変更されてから一定時間経過してから、変更があるか否かを判定する。
        private const int LastTextChangedTimeoutMs = 500;
        private readonly System.Windows.Forms.Timer _lastTextChangedTimer = new()
        {
            Interval = LastTextChangedTimeoutMs
        };

        public KeyConfirmForm()
        {
            InitializeComponent();
            _lastTextChangedTimer.Tick += LastTextChangedTimer_OnTick;
            _lastTextChangedTimer.Stop();
        }

        private void TouchArea_OnMouseDown(object sender, MouseEventArgs e)
        {
            var touchAreaLabel = (TouchAreaLabel)sender;
            touchAreaLabel.IsSelected = !touchAreaLabel.IsSelected;
            touchAreaLabel.Invalidate(); // 再描画

            TouchAreaLabel[] touchAreaLabels = {
                touchAreaLabelCenter,
                touchAreaLabelUL,
                touchAreaLabelUR,
                touchAreaLabelLL,
                touchAreaLabelLR
            };
            var selectedLabels = touchAreaLabels.Where(e => e.IsSelected).ToArray();
            if (selectedLabels.Length == 1)
            {
                inputKeyLabel.Text = selectedLabels[0].Name switch
                {
                    "touchAreaLabelCenter" => "F13",
                    "touchAreaLabelUL" => "F14",
                    "touchAreaLabelUR" => "F15",
                    "touchAreaLabelLL" => "F16",
                    "touchAreaLabelLR" => "F17",
                    _ => "無し"
                };
            }
            else if (selectedLabels.Length == 2)
            {
                inputKeyLabel.Text = (selectedLabels[0].Name, selectedLabels[1].Name) switch
                {
                    ("touchAreaLabelUL", "touchAreaLabelUR") or ("touchAreaLabelUR", "touchAreaLabelUL") => "F18",
                    ("touchAreaLabelUL", "touchAreaLabelLL") or ("touchAreaLabelLL", "touchAreaLabelUL") => "F19",
                    ("touchAreaLabelUL", "touchAreaLabelLR") or ("touchAreaLabelLR", "touchAreaLabelUL") => "F20",
                    ("touchAreaLabelUR", "touchAreaLabelLL") or ("touchAreaLabelLL", "touchAreaLabelUR") => "F21",
                    ("touchAreaLabelUR", "touchAreaLabelLR") or ("touchAreaLabelLR", "touchAreaLabelUR") => "F22",
                    ("touchAreaLabelLL", "touchAreaLabelLR") or ("touchAreaLabelLR", "touchAreaLabelLL") => "F23",
                    _ => "無し"
                };
            }
            else
            {
                inputKeyLabel.Text = "無し";
            }
            inputKeyLabel.Invalidate(); // 再描画

            if (inputKeyLabel.Text != "無し" && _ahkFileContent != string.Empty)
            {
                // <Key>::の前に半角スペース・タブはいくつ入っていてもOK（全角スペースが入っているとAHKがエラーを吐く）。
                var hotkeyPat = $@"[ \t]*{inputKeyLabel.Text}::";
                var match = Regex.Match(_ahkFileContent, hotkeyPat);
                if (match.Success)
                {
                    fileEditTBox.Focus();
                    // ScrollToCaret()は、キャレットのある行まで最小のスクロールを行う。
                    // なので、マッチ行がTextBoxの一番上になるようにスクロールさせるには、下側からスクロールさせる必要がある。
                    fileEditTBox.Select(_ahkFileContent.Length, 0);
                    fileEditTBox.ScrollToCaret();
                    fileEditTBox.Select(match.Index, 0);
                    fileEditTBox.ScrollToCaret();
                }
            }
        }

        private void OpenDialog_OnClick(object sender, EventArgs e)
        {
            if (IsTextChanged())
            {
                var answer = MessageBox.Show("変更内容を保存しますか？",
                    "KeyConfirmForm", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    SaveInFile();
                }
                else if (answer == DialogResult.Cancel)
                {
                    return;
                }
            }

            var initDir = (UserSettings.Instance.AhkFilePath == string.Empty) ? @"C:\" : Path.GetDirectoryName(UserSettings.Instance.AhkFilePath);
            var initFileName = (UserSettings.Instance.AhkFilePath == string.Empty) ? string.Empty : Path.GetFileName(UserSettings.Instance.AhkFilePath);
            using var openFileDialog = new OpenFileDialog
            {
                InitialDirectory = initDir,
                FileName = initFileName,
                Filter = "AutoHotkey Script (*.ahk)|*.ahk",
                Title = "AHKファイルを選択",
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            _fileStream?.Dispose();
            try
            {
                // 他のプロセスからの書き込みを禁止して開く
                // あえて開きっぱなしにすることで、ファイルをロックする
                _fileStream = new FileStream(
                    openFileDialog.FileName/* ファイル選択後のFileNameにはフルパスが入る */,
                    FileMode.Open,
                    FileAccess.ReadWrite,
                    FileShare.Read
                );

                // leaveOpenをtrueにすると、StreamReaderがDisposeされても、基となるFileStreamはDisposeされない。
                using var sReader = new StreamReader(_fileStream, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
                _currentEncoding = sReader.CurrentEncoding;
                _ahkFileContent = sReader.ReadToEnd();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("指定されたファイルが見つかりません。",
                    "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("指定されたパスが見つかりません。",
                    "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("このファイルへのアクセスはOSによって許可されていません。",
                    "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
            catch (SecurityException ex)
            {
                MessageBox.Show("このファイルへのアクセスは許可されていません。",
                   "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
                return;
            }
 
            // TextBoxのTextに値をセットした際、内部でInvalidate()が呼ばれているらしい。
            fileEditTBox.Text = _ahkFileContent;
            filePathTBox.Text = openFileDialog.FileName;
            UserSettings.Instance.AhkFilePath = openFileDialog.FileName;
        }

        // 以下の2パターンでファイルに保存する。
        // 1. TextBoxに書き込まれた内容を新規ファイルに保存する
        // 2. 既に開かれているファイルに上書き保存する
        private void SaveInFile()
        {
            // ファイル名が指定されていない場合
            if (filePathTBox.Text == string.Empty)
            {
                var initDir = (UserSettings.Instance.AhkFilePath == string.Empty) ? @"C:\" : Path.GetDirectoryName(UserSettings.Instance.AhkFilePath);
                using var saveFileDialog = new SaveFileDialog
                {
                    InitialDirectory = initDir,
                    Filter = "AutoHotkey Script (*.ahk)|*.ahk",
                    Title = "AHKファイルを作成",
                    FileName = "新しいファイル.ahk",
                    RestoreDirectory = true
                };
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

                _currentEncoding = new UTF8Encoding(false); // デフォルトではUTF-8（BOM無し）に設定する
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, fileEditTBox.Text, _currentEncoding);
                }
                catch (DirectoryNotFoundException ex) 
                {
                    MessageBox.Show("指定されたパスが見つかりません。",
                        "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("このファイルは書き込み可能ではありません。",
                        "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show("このファイルへのアクセスは許可されていません。",
                        "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }
                filePathTBox.Text = saveFileDialog.FileName;
                UserSettings.Instance.AhkFilePath = saveFileDialog.FileName;
                _ahkFileContent = fileEditTBox.Text;
                // ファイル内容が最新状態になるため
                ToggleBtnEnabled(false);
            }
            else
            {
                _fileStream?.SetLength(0); // ファイル内容を破棄する
                _fileStream?.Position = 0; // シーク位置を書き込み開始位置＝先頭に戻す
                try
                {
                    using var sWriter = new StreamWriter(_fileStream!, _currentEncoding!, leaveOpen: true);
                    sWriter.Write(fileEditTBox.Text);
                    sWriter.Flush(); // バッファの内容をファイルに書き込む
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show("このファイルは書き込み可能ではありません。",
                        "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }
                catch (ObjectDisposedException ex)
                {
                    MessageBox.Show("バッファがいっぱいで書き込めません。",
                        "KeyConfirmForm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine(ex.Message);
                    return;
                }
                _ahkFileContent = fileEditTBox.Text;
                // ファイル内容が最新状態になるため
                ToggleBtnEnabled(false);
            }
        }

        private void Save_OnClick(object sender, EventArgs e)
        {
            SaveInFile();
        }

        private bool IsTextChanged()
        {
            return (filePathTBox.Text == string.Empty && fileEditTBox.Text != string.Empty)
                || fileEditTBox.Text != _ahkFileContent;
        }

        private void DiscardChange_OnClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("変更内容を破棄しますか？",
                "KeyConfirmForm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (filePathTBox.Text == string.Empty)
                {
                    fileEditTBox.Text = string.Empty;
                }
                else
                {
                    fileEditTBox.Text = _ahkFileContent;
                }
            }
        }

        // フォームが閉じられる前に行われる処理
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsTextChanged())
            {
                var answer = MessageBox.Show("変更内容を保存しますか？",
                    "KeyConfirmForm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    SaveInFile();
                }
                else if (answer == DialogResult.Cancel)
                {
                    e.Cancel = true; // フォームのクローズをキャンセルする
                    return;
                }
            }
            _lastTextChangedTimer.Dispose();
            _fileStream?.Dispose(); // ファイルのロックを解除
            UserSettings.Instance.Save(); // ユーザー設定をconfigファイルに保存する
        }

        private void EditTBox_OnTextChanged(object sender, EventArgs e)
        {
            _lastTextChangedTimer.Stop();
            _lastTextChangedTimer.Start();
        }

        private void ToggleBtnEnabled(bool isTextChanged)
        {
            saveBtn.Enabled = isTextChanged;
            discardChangeBtn.Enabled = isTextChanged;
        }

        // ファイル内容が変更されているか否かが一目でわかるようにする。
        private void LastTextChangedTimer_OnTick(object? sender, EventArgs e)
        {
            _lastTextChangedTimer.Stop();
            ToggleBtnEnabled(IsTextChanged());
        }
    }
}