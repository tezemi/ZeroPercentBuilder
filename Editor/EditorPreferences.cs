using System.Collections.Generic;
using UnityEditor;
using ZeroPercentBuilder.Utilities;

namespace ZeroPercentBuilder
{
    public static class PipelinePreferences
    {
        private const string SteamCMDPathKey = "ZeroPercentBuilder.SteamCMDPath";
        private const string LogDirectoryKey = "ZeroPercentBuilder.LogDirectory";

        public static string SteamCMDPath
        {
            get => EditorPrefs.GetString(SteamCMDPathKey, string.Empty);
            set => EditorPrefs.SetString(SteamCMDPathKey, value);
        }

        public static string LogDirectory
        {
            get => EditorPrefs.GetString(LogDirectoryKey, string.Empty);
            set => EditorPrefs.SetString(LogDirectoryKey, value);
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider() => new SettingsProvider("Preferences/Zero Percent Builder", SettingsScope.User)
        {
            label = "Zero Percent Builder",
            guiHandler = _ =>
            {
                EditorGUILayout.Space();
                SteamCMDPath = EditorGUIUtilities.FolderPicker("Steam CMD", SteamCMDPath, s => SteamCMDPath = s);
                LogDirectory = EditorGUIUtilities.FolderPicker("Log Directory", LogDirectory, s => LogDirectory = s);
            },
            keywords = new HashSet<string> { "Pipeline", "Steam", "Deploy" }
        };
    }
}

