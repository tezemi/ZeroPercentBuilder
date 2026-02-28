using System;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.BuildSources;

namespace ZeroPercentBuilder.CustomEditors
{
    [CustomEditor(typeof(BuildConfig))]
    public class BuildConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BuildConfig buildConfig = (BuildConfig)target;
            
            EditorGUI.BeginChangeCheck();

            buildConfig.ProductName = EditorGUIUtilities.TextFieldWithPlaceHolder("Product Name", buildConfig.ProductName, Application.productName);
            buildConfig.Version = EditorGUIUtilities.TextFieldWithPlaceHolder("Version", buildConfig.Version, Application.version);
            buildConfig.ProgramName = EditorGUIUtilities.TextFieldWithPlaceHolder("Program Name", buildConfig.ProgramName, BuildUtilities.GetExecutableName(Application.productName, buildConfig.BuildTarget));
            
            EditorGUILayout.Space();

            buildConfig.BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", buildConfig.BuildTarget);
            buildConfig.Architecture = EditorGUIUtilities.ArchitecturePopup("Architecture", buildConfig.Architecture);
            buildConfig.ScriptingImplementation = (ScriptingImplementation)EditorGUILayout.EnumPopup("Scripting Implementation", buildConfig.ScriptingImplementation);
            buildConfig.ManagedStrippingLevel = (ManagedStrippingLevel)EditorGUILayout.EnumPopup("Stripping Level", buildConfig.ManagedStrippingLevel);

            EditorGUILayout.Space();

            bool allowDebugging;
            bool deepProfiling;
            bool autoConnectProfiler;
            bool developmentBuild = EditorGUILayout.Toggle("Development Build", buildConfig.BuildOptions.HasFlag(BuildOptions.Development));
            
            using (new EditorGUI.DisabledScope(!developmentBuild))
            {
                EditorGUI.indentLevel++;
                allowDebugging = EditorGUILayout.Toggle("Allow Debugging", buildConfig.BuildOptions.HasFlag(BuildOptions.AllowDebugging));
                EditorGUI.indentLevel--;
            }

            using (new EditorGUI.DisabledScope(!developmentBuild))
            {
                EditorGUI.indentLevel++;
                deepProfiling = EditorGUILayout.Toggle("Deep Profiling", buildConfig.BuildOptions.HasFlag(BuildOptions.EnableDeepProfilingSupport));
                EditorGUI.indentLevel--;
            }

            using (new EditorGUI.DisabledScope(!developmentBuild))
            {
                EditorGUI.indentLevel++;
                autoConnectProfiler = EditorGUILayout.Toggle("Auto Connect Profiler", buildConfig.BuildOptions.HasFlag(BuildOptions.ConnectWithProfiler));
                EditorGUI.indentLevel--;
            }

            if (developmentBuild) 
                buildConfig.BuildOptions |= BuildOptions.Development;
            else
                buildConfig.BuildOptions &= ~BuildOptions.Development;
            
            if (developmentBuild && allowDebugging)   
                buildConfig.BuildOptions |= BuildOptions.AllowDebugging;
            else
                buildConfig.BuildOptions &= ~BuildOptions.AllowDebugging;

            if (developmentBuild && deepProfiling)   
                buildConfig.BuildOptions |= BuildOptions.EnableDeepProfilingSupport;
            else
                buildConfig.BuildOptions &= ~BuildOptions.EnableDeepProfilingSupport;
            
            if (developmentBuild && autoConnectProfiler)   
                buildConfig.BuildOptions |= BuildOptions.ConnectWithProfiler;
            else
                buildConfig.BuildOptions &= ~BuildOptions.ConnectWithProfiler;
            

            EditorGUILayout.Space();

            bool cleanBuild = EditorGUILayout.Toggle("Clean Build", buildConfig.BuildOptions.HasFlag(BuildOptions.CleanBuildCache));

            if (cleanBuild)       
                buildConfig.BuildOptions |= BuildOptions.CleanBuildCache;
            else
                buildConfig.BuildOptions &= ~BuildOptions.CleanBuildCache;
            
            EditorGUILayout.Space();
            
            buildConfig.Scenes = EditorGUIUtilities.SceneList("Scenes", buildConfig.Scenes);

            if (buildConfig.ScriptDefines == null)
                buildConfig.ScriptDefines = Array.Empty<string>();

            EditorGUIUtilities.StringArray("Script Defines", ref buildConfig.ScriptDefines);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}