namespace TouchpadRecognizer
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            bool createNew;
            using Mutex mutex = new(true, "TouchpadRecognizer", out createNew);
            if (!createNew)
            {
                MessageBox.Show("すでに起動しています。",
                    "TouchpadRecognizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 非表示状態でフォームを生成する。
            ApplicationConfiguration.Initialize();
            using var recognizer = new TouchpadRecognizerForm();
            Application.Run();
        }
    }
}