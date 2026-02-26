using UnityEditor;
using UnityEngine;
using ZeroPercentBuilder.BuildSources;
using ZeroPercentBuilder.Utilities;

namespace ZeroPercentBuilder.CustomEditors
{
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BuildConfig buildConfig = (BuildConfig)target;
            
            buildConfig.ProductName = EditorGUIUtilities.TextFieldWithPlaceHolder("Product Name", buildConfig.ProductName, Application.productName);
            buildConfig.Version = EditorGUIUtilities.TextFieldWithPlaceHolder("Version", buildConfig.Version, Application.version);
            buildConfig.ProgramName = EditorGUIUtilities.TextFieldWithPlaceHolder("Program Name", buildConfig.ProgramName, BuildUtilities.GetExecutableName(Application.productName, buildConfig.BuildTarget));
            
            buildConfig.BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", buildConfig.BuildTarget);
            
            bool developmentBuild = EditorGUILayout.Toggle("Development Build", buildConfig.BuildOptions.HasFlag(BuildOptions.Development));
            bool allowDebugging;
            
            using (new EditorGUI.DisabledScope(!developmentBuild))
            {
                EditorGUI.indentLevel++;
                allowDebugging = EditorGUILayout.Toggle("Allow Debugging", buildConfig.BuildOptions.HasFlag(BuildOptions.AllowDebugging));
                EditorGUI.indentLevel--;
            }

            bool cleanBuild = EditorGUILayout.Toggle("Clean Build", buildConfig.BuildOptions.HasFlag(BuildOptions.CleanBuildCache));
            
            if (developmentBuild) 
                buildConfig.BuildOptions |= BuildOptions.Development;
            else
                buildConfig.BuildOptions &= ~BuildOptions.Development;
            
            if (developmentBuild && allowDebugging)   
                buildConfig.BuildOptions |= BuildOptions.AllowDebugging;
            else
                buildConfig.BuildOptions &= ~BuildOptions.AllowDebugging;
            
            if (cleanBuild)       
                buildConfig.BuildOptions |= BuildOptions.CleanBuildCache;
            else
                buildConfig.BuildOptions &= ~BuildOptions.CleanBuildCache;
            
            buildConfig.Scenes = EditorGUIUtilities.SceneList("Scenes", buildConfig.Scenes);
        }
    }
}