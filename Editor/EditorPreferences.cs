using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using ZeroPercentBuilder.Utilities;

namespace ZeroPercentBuilder
{
    public static class PipelinePreferences
    {
        private const string SteamCMDPathKey = "ZeroPercentBuilder.SteamCMDPath";
        private const string SteamUsernameKey = "ZeroPercentBuilder.SteamUsername";
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
        
        public static string SteamUsername
        {
            get => EditorPrefs.GetString(SteamUsernameKey, string.Empty);
            set => EditorPrefs.SetString(SteamUsernameKey, value);
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider() => new SettingsProvider("Preferences/Zero Percent Builder", SettingsScope.User)
        {
            label = "Zero Percent Builder",
            guiHandler = _ =>
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                SteamCMDPath = EditorGUIUtilities.FilePicker("Steam CMD", SteamCMDPath, s => SteamCMDPath = s);
                if (GUILayout.Button("Cache Login", GUILayout.Width(100f)))
                {
                    string steamCMDCommand = $"\"{SteamCMDPath}\" +login {SteamUsername} +quit";
    
                    ProcessStartInfo startInfo;

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        startInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/k {steamCMDCommand}"
                        };
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        startInfo = new ProcessStartInfo
                        {
                            FileName = "osascript",
                            Arguments = $"-e 'tell app \"Terminal\" to do script \"{steamCMDCommand}\"'"
                        };
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        startInfo = new ProcessStartInfo
                        {
                            FileName = "x-terminal-emulator",
                            Arguments = $"-e {steamCMDCommand}"
                        };
                    }
                    else
                    {
                        throw new Exception("Could not determine OS platform. Run SteamCMD manually instead.");
                    }

                    Process.Start(startInfo);
                }
                EditorGUILayout.EndHorizontal();
                LogDirectory = EditorGUIUtilities.FolderPicker("Log Directory", LogDirectory, s => LogDirectory = s);
                SteamUsername = EditorGUILayout.TextField("Steam Username", SteamUsername);
            },
            keywords = new HashSet<string> { "Pipeline", "Steam", "Deploy" }
        };
    }
}

