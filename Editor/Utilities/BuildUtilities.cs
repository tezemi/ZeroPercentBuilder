using UnityEditor;

namespace ZeroPercentBuilder.Utilities
{
    public class BuildUtilities
    {
        public static string GetExecutableName(string executableName, BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return executableName + ".exe";
                case BuildTarget.StandaloneLinux64:
                    return executableName + ".x86_64";
                case BuildTarget.StandaloneOSX:
                    return executableName + ".app";
                case BuildTarget.Android:
                    return executableName + ".apk";
                case BuildTarget.iOS:
                    return executableName;
                case BuildTarget.WebGL:
                    return "index.html";
                default:
                    return executableName;
            }
        }
    }
}