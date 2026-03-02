using UnityEditor.Build;

namespace ZeroPercentBuilder
{
    public static class PipelineBuildContext
    {
        public static NamedBuildTarget ActiveBuildTarget { get; set; }
        public static string[] ActiveScriptingDefines { get; set; }
    }
}