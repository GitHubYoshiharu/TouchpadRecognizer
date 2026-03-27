using System.Runtime.InteropServices;

namespace TouchpadRecognizer
{
    internal static class TouchpadHelper
    {
        // 2本の指でどの位置をタップしたかを表す。
        public enum TouchPos
        {
            CENTER,
            UL,
            UR,
            LL,
            LR,
            UL_UR,
            UL_LL,
            UL_LR,
            UR_LL,
            UR_LR,
            LL_LR
        }

        // タッチ座標が取り得る値の最大値
        // input reportのvalueCapからしか取得できないため、やむなくここで設定する。
        private static int? LogicalMaxX;
        private static int? LogicalMaxY;

        // 2つのContactの位置関係から、TouchPosを判定する。
        public static TouchPos? JudgeTouchPos((int X, int Y) con1, (int X, int Y) con2)
        {
            if (!LogicalMaxX.HasValue || !LogicalMaxY.HasValue) return null;
            var centerX = (double)LogicalMaxX / 2.0;
            var centerY = (double)LogicalMaxY / 2.0;
            
            //--- 中央領域は他の領域と重複しているので、先に判定する。
            var centerDiameter = Math.Min((double)LogicalMaxX, (double)LogicalMaxY) * UserSettings.Instance.CenterDiameterRatio/100;
            // 2.0乗で符号が消えるかどうかがわからないので、念の為に絶対値を取っている。
            var d1 = (X: Math.Pow(Math.Abs(con1.X - centerX), 2.0), Y: Math.Pow(Math.Abs(con1.Y - centerY), 2.0));
            var d2 = (X: Math.Pow(Math.Abs(con2.X - centerX), 2.0), Y: Math.Pow(Math.Abs(con2.Y - centerY), 2.0));
            if (Math.Sqrt(d1.X + d1.Y) <= centerDiameter/2 && Math.Sqrt(d2.X + d2.Y) <= centerDiameter/2)
            {
                return TouchPos.CENTER;
            }

            var pos1 = JudgeAreaOfContact(con1, centerX, centerY);
            var pos2 = JudgeAreaOfContact(con2, centerX, centerY);
            return (pos1, pos2) switch
            {
                (TouchPos.UL, TouchPos.UL) => TouchPos.UL,
                (TouchPos.UR, TouchPos.UR) => TouchPos.UR,
                (TouchPos.LL, TouchPos.LL) => TouchPos.LL,
                (TouchPos.LR, TouchPos.LR) => TouchPos.LR,
                (TouchPos.UL, TouchPos.UR) or (TouchPos.UR, TouchPos.UL) => TouchPos.UL_UR,
                (TouchPos.UL, TouchPos.LL) or (TouchPos.LL, TouchPos.UL) => TouchPos.UL_LL,
                (TouchPos.UL, TouchPos.LR) or (TouchPos.LR, TouchPos.UL) => TouchPos.UL_LR,
                (TouchPos.UR, TouchPos.LL) or (TouchPos.LL, TouchPos.UR) => TouchPos.UR_LL,
                (TouchPos.UR, TouchPos.LR) or (TouchPos.LR, TouchPos.UR) => TouchPos.UR_LR,
                _ => TouchPos.LL_LR,
            };
        }

        private static TouchPos JudgeAreaOfContact((int X, int Y) con, double centerX, double centerY)
        {
            if (con.X < centerX && con.Y < centerY) return TouchPos.UL;
            if (con.X >= centerX && con.Y < centerY) return TouchPos.UR;
            if (con.X < centerX && con.Y >= centerY) return TouchPos.LL;
            return TouchPos.LR;
        }

        // ハイブリッドモードのタッチパッドは、1つのHIDレポートで1つのコンタクトのデータしか送信しないので、
        // scanTimeを用いて同一フレームのコンタクトをグルーピングする。
        public static (uint ScanTime, TouchpadContact[]? Contacts) ParseInputWithScanTime(IntPtr lParam)
        {
            // Get RAWINPUT.
            uint rawInputSize = 0;
            uint rawInputHeaderSize = (uint)Marshal.SizeOf<RAWINPUTHEADER>();

            if (GetRawInputData(
                lParam,
                RID_INPUT,
                IntPtr.Zero,
                ref rawInputSize,
                rawInputHeaderSize) != 0)
            {
                return (0, null);
            }

            RAWINPUT rawInput;
            byte[] rawHidRawData;

            IntPtr rawInputPointer = IntPtr.Zero;
            try
            {
                rawInputPointer = Marshal.AllocHGlobal((int)rawInputSize);

                if (GetRawInputData(
                    lParam,
                    RID_INPUT,
                    rawInputPointer,
                    ref rawInputSize,
                    rawInputHeaderSize) != rawInputSize)
                {
                    return (0, null);
                }

                rawInput = Marshal.PtrToStructure<RAWINPUT>(rawInputPointer);

                var rawInputData = new byte[rawInputSize];
                Marshal.Copy(rawInputPointer, rawInputData, 0, rawInputData.Length);

                rawHidRawData = new byte[rawInput.Hid.dwSizeHid * rawInput.Hid.dwCount];
                int rawInputOffset = (int)rawInputSize - rawHidRawData.Length;
                Buffer.BlockCopy(rawInputData, rawInputOffset, rawHidRawData, 0, rawHidRawData.Length);
            }
            finally
            {
                if (rawInputPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(rawInputPointer);
            }

            // Parse RAWINPUT.
            IntPtr rawHidRawDataPointer = Marshal.AllocHGlobal(rawHidRawData.Length);
            Marshal.Copy(rawHidRawData, 0, rawHidRawDataPointer, rawHidRawData.Length);

            IntPtr preparsedDataPointer = IntPtr.Zero;
            try
            {
                uint preparsedDataSize = 0;

                if (GetRawInputDeviceInfo(
                    rawInput.Header.hDevice,
                    RIDI_PREPARSEDDATA,
                    IntPtr.Zero,
                    ref preparsedDataSize) != 0)
                {
                    return (0, null);
                }

                preparsedDataPointer = Marshal.AllocHGlobal((int)preparsedDataSize);

                if (GetRawInputDeviceInfo(
                    rawInput.Header.hDevice,
                    RIDI_PREPARSEDDATA,
                    preparsedDataPointer,
                    ref preparsedDataSize) != preparsedDataSize)
                {
                    return (0, null);
                }
                
                if (HidP_GetCaps(
                    preparsedDataPointer,
                    out HIDP_CAPS caps) != HIDP_STATUS_SUCCESS)
                {
                    return (0, null);
                }

                ushort valueCapsLength = caps.NumberInputValueCaps;
                var valueCaps = new HIDP_VALUE_CAPS[valueCapsLength];

                if (HidP_GetValueCaps(
                    HIDP_REPORT_TYPE.HidP_Input,
                    valueCaps,
                    ref valueCapsLength,
                    preparsedDataPointer) != HIDP_STATUS_SUCCESS)
                {
                    return (0, null);
                }

                uint scanTime = 0;
                uint contactCount = 0;
                TouchpadContactCreator creator = new();
                List<TouchpadContact> contacts = new();

                foreach (var valueCap in valueCaps.OrderBy(x => x.LinkCollection))
                {
                    if (HidP_GetUsageValue(
                        HIDP_REPORT_TYPE.HidP_Input,
                        valueCap.UsagePage,
                        valueCap.LinkCollection,
                        valueCap.Usage,
                        out uint value,
                        preparsedDataPointer,
                        rawHidRawDataPointer,
                        (uint)rawHidRawData.Length) != HIDP_STATUS_SUCCESS)
                    {
                        continue;
                    }

                    switch (valueCap.LinkCollection)
                    {
                        case 0:
                            switch (valueCap.UsagePage, valueCap.Usage)
                            {
                                case (0x0D, 0x56): // Scan Time
                                    scanTime = value;
                                    break;

                                case (0x0D, 0x54): // Contact Count
                                    contactCount = value;
                                    break;
                            }
                            break;

                        default:
                            switch (valueCap.UsagePage, valueCap.Usage)
                            {
                                case (0x0D, 0x51): // Contact ID
                                    creator.ContactId = (int)value;
                                    break;

                                case (0x01, 0x30): // X
                                    creator.X = (int)value;
                                    if(!LogicalMaxX.HasValue) LogicalMaxX = valueCap.LogicalMax;
                                    break;

                                case (0x01, 0x31): // Y
                                    creator.Y = (int)value;
                                    if (!LogicalMaxY.HasValue) LogicalMaxY = valueCap.LogicalMax;
                                    break;
                            }
                            break;
                    }

                    if (creator.TryCreate(out TouchpadContact contact))
                    {
                        contacts.Add(contact);
                        if (contacts.Count >= contactCount) break;

                        creator.Clear();
                    }
                }

                return (scanTime, contacts.ToArray());
            }
            finally
            {
                Marshal.FreeHGlobal(rawHidRawDataPointer);
                if (preparsedDataPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(preparsedDataPointer);
            }
        }


        [DllImport("User32.dll", SetLastError = true)]
        private static extern uint GetRawInputData(
            IntPtr hRawInput, // lParam in WM_INPUT
            uint uiCommand, // RID_HEADER
            IntPtr pData,
            ref uint pcbSize,
            uint cbSizeHeader);

        private const uint RID_INPUT = 0x10000003;

        [StructLayout(LayoutKind.Sequential)]
        private struct RAWINPUT
        {
            public RAWINPUTHEADER Header;
            public RAWHID Hid;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RAWINPUTHEADER
        {
            public uint dwType; // RIM_TYPEMOUSE or RIM_TYPEKEYBOARD or RIM_TYPEHID
            public uint dwSize;
            public IntPtr hDevice;
            public IntPtr wParam; // wParam in WM_INPUT
        }

        private const uint RIM_TYPEMOUSE = 0;
        private const uint RIM_TYPEKEYBOARD = 1;
        private const uint RIM_TYPEHID = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct RAWHID
        {
            public uint dwSizeHid;
            public uint dwCount;
            public IntPtr bRawData; // This is not for use.
        }

        [DllImport("User32.dll", SetLastError = true)]
        private static extern uint GetRawInputDeviceInfo(
            IntPtr hDevice, // hDevice by RAWINPUTHEADER
            uint uiCommand, // RIDI_PREPARSEDDATA
            IntPtr pData,
            ref uint pcbSize);

        private const uint RIDI_PREPARSEDDATA = 0x20000005;

        [DllImport("Hid.dll", SetLastError = true)]
        private static extern uint HidP_GetCaps(
            IntPtr PreparsedData,
            out HIDP_CAPS Capabilities);

        private const uint HIDP_STATUS_SUCCESS = 0x00110000;

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDP_CAPS
        {
            public ushort Usage;
            public ushort UsagePage;
            public ushort InputReportByteLength;
            public ushort OutputReportByteLength;
            public ushort FeatureReportByteLength;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public ushort[] Reserved;

            public ushort NumberLinkCollectionNodes;
            public ushort NumberInputButtonCaps;
            public ushort NumberInputValueCaps;
            public ushort NumberInputDataIndices;
            public ushort NumberOutputButtonCaps;
            public ushort NumberOutputValueCaps;
            public ushort NumberOutputDataIndices;
            public ushort NumberFeatureButtonCaps;
            public ushort NumberFeatureValueCaps;
            public ushort NumberFeatureDataIndices;
        }

        [DllImport("Hid.dll", CharSet = CharSet.Auto)]
        private static extern uint HidP_GetValueCaps(
            HIDP_REPORT_TYPE ReportType,
            [Out] HIDP_VALUE_CAPS[] ValueCaps,
            ref ushort ValueCapsLength,
            IntPtr PreparsedData);

        private enum HIDP_REPORT_TYPE
        {
            HidP_Input,
            HidP_Output,
            HidP_Feature
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDP_VALUE_CAPS
        {
            public ushort UsagePage;
            public byte ReportID;

            [MarshalAs(UnmanagedType.U1)]
            public bool IsAlias;

            public ushort BitField;
            public ushort LinkCollection;
            public ushort LinkUsage;
            public ushort LinkUsagePage;

            [MarshalAs(UnmanagedType.U1)]
            public bool IsRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsStringRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsDesignatorRange;
            [MarshalAs(UnmanagedType.U1)]
            public bool IsAbsolute;
            [MarshalAs(UnmanagedType.U1)]
            public bool HasNull;

            public byte Reserved;
            public ushort BitSize;
            public ushort ReportCount;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ushort[] Reserved2;

            public uint UnitsExp;
            public uint Units;
            public int LogicalMin;
            public int LogicalMax;
            public int PhysicalMin;
            public int PhysicalMax;

            // Range
            public ushort UsageMin;
            public ushort UsageMax;
            public ushort StringMin;
            public ushort StringMax;
            public ushort DesignatorMin;
            public ushort DesignatorMax;
            public ushort DataIndexMin;
            public ushort DataIndexMax;

            // NotRange
            public ushort Usage => UsageMin;
            public ushort StringIndex => StringMin;
            public ushort DesignatorIndex => DesignatorMin;
            public ushort DataIndex => DataIndexMin;
        }

        [DllImport("Hid.dll", CharSet = CharSet.Auto)]
        private static extern uint HidP_GetUsageValue(
            HIDP_REPORT_TYPE ReportType,
            ushort UsagePage,
            ushort LinkCollection,
            ushort Usage,
            out uint UsageValue,
            IntPtr PreparsedData,
            IntPtr Report,
            uint ReportLength);
    }
}
