using System.Configuration;

namespace TouchpadRecognizer
{
    // このクラスのプロパティがuser.configに保存される。
    internal sealed class UserSettings : ApplicationSettingsBase
    {
        private static readonly Lazy<UserSettings> _instance =
        new(() => new UserSettings());

        public static UserSettings Instance => _instance.Value;

        private UserSettings() { }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string AhkFilePath
        {
            get => (string)this["AhkFilePath"];
            set => this["AhkFilePath"] = value;
        }

        // タッチパッドの短辺の長さの何%を中央領域の直径にするか
        [UserScopedSetting()]
        [DefaultSettingValue("60")]
        public int CenterDiameterRatio
        {
            get => (int)this["CenterDiameterRatio"];
            set => this["CenterDiameterRatio"] = value;
        }

        // タッチパッドへの入力が無くなってから全ての指が離れたとみなすまでの時間（ms）
        [UserScopedSetting()]
        [DefaultSettingValue("60")]
        public int InactivityTimeoutMs
        {
            get => (int)this["InactivityTimeoutMs"];
            set => this["InactivityTimeoutMs"] = value;
        }

        // 2本指での同時押しの許容時間差（ms）
        [UserScopedSetting()]
        [DefaultSettingValue("100")]
        public int AcceptableDelayMs
        {
            get => (int)this["AcceptableDelayMs"];
            set => this["AcceptableDelayMs"] = value;
        }

        // タップとみなす、指が触れてから離すまでの最大時間（ms）
        [UserScopedSetting()]
        [DefaultSettingValue("250")]
        public int TapTimeThresholdMs
        {
            get => (int)this["TapTimeThresholdMs"];
            set => this["TapTimeThresholdMs"] = value;
        }

        // タップとみなす、指が触れてからの最大移動距離（px）
        [UserScopedSetting()]
        [DefaultSettingValue("40")]
        public int TapDistanceThresholdPx
        {
            get => (int)this["TapDistanceThresholdPx"];
            set => this["TapDistanceThresholdPx"] = value;
        }
    }
}