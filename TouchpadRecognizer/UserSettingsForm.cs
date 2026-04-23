namespace TouchpadRecognizer
{
    public partial class UserSettingsForm : Form
    {
        public UserSettingsForm()
        {
            InitializeComponent();
        }

        private void Reset_OnClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("全ての値を既定値に戻しますか？",
                "UserSettingsForm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var tmp = UserSettings.Instance.AhkFilePath;
                UserSettings.Instance.Reset();
                UserSettings.Instance.AhkFilePath = tmp;
                centerDiameterRatioNud.Value = UserSettings.Instance.CenterDiameterRatio;
                inactivityTimeoutMsNud.Value = UserSettings.Instance.InactivityTimeoutMs;
                acceptableDelayMsNud.Value = UserSettings.Instance.AcceptableDelayMs;
                tapTimeThresholdMsNud.Value = UserSettings.Instance.TapTimeThresholdMs;
                tapDistanceThresholdPxNud.Value = UserSettings.Instance.TapDistanceThresholdPx;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            UserSettings.Instance.CenterDiameterRatio = (int)centerDiameterRatioNud.Value;
            UserSettings.Instance.InactivityTimeoutMs = (int)inactivityTimeoutMsNud.Value;
            UserSettings.Instance.AcceptableDelayMs = (int)acceptableDelayMsNud.Value;
            UserSettings.Instance.TapTimeThresholdMs = (int)tapTimeThresholdMsNud.Value;
            UserSettings.Instance.TapDistanceThresholdPx = (int)tapDistanceThresholdPxNud.Value;
            UserSettings.Instance.Save();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            centerDiameterRatioNud.Value = UserSettings.Instance.CenterDiameterRatio;
            inactivityTimeoutMsNud.Value = UserSettings.Instance.InactivityTimeoutMs;
            acceptableDelayMsNud.Value = UserSettings.Instance.AcceptableDelayMs;
            tapTimeThresholdMsNud.Value = UserSettings.Instance.TapTimeThresholdMs;
            tapDistanceThresholdPxNud.Value = UserSettings.Instance.TapDistanceThresholdPx;
        }
    }
}
