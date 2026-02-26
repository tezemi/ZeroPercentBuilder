using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.CustomEditors
{
    [CustomEditor(typeof(Pipeline))]
    public class PipelineEditor : Editor
    {        
        private readonly Dictionary<IBuildStep, bool> _foldouts = new ();

        public override void OnInspectorGUI()
        {
            Pipeline pipeline = (Pipeline)target;

            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.LabelField(pipeline.name, EditorStyles.boldLabel);

            EditorGUILayout.Space();

            pipeline.LogToFile = EditorGUILayout.Toggle("Log to File", pipeline.LogToFile);
            if (pipeline.LogToFile)
            {
                pipeline.LogFileDirectory = EditorGUIUtilities.FolderPicker("Log Directory", pipeline.LogFileDirectory, d => pipeline.LogFileDirectory = d);
            }

            EditorGUILayout.Space();

            if (pipeline.BuildSteps != null)
            {
                for (int i = 0; i < pipeline.BuildSteps.Count; i++)
                {
                    IBuildStep step = pipeline.BuildSteps[i];

                    if (!_foldouts.ContainsKey(step))
                    {
                        _foldouts[step] = false;
                    }

                    EditorGUILayout.BeginHorizontal();

                    _foldouts[step] = EditorGUILayout.Foldout
                    (
                        _foldouts[step], 
                        $"{i + 1}. {BuildStepUtilities.GetBuildStepName(step)}",
                        true, 
                        EditorStyles.foldoutHeader
                    );

                    if (GUILayout.Button("…", GUILayout.Width(30f)))
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Move to Top"), false, () => EditorApplication.delayCall += () => pipeline.BuildSteps.MoveToFirst(step));
                        menu.AddItem(new GUIContent("Move Up"), false, () => EditorApplication.delayCall += () => pipeline.BuildSteps.MoveBack(step));
                        menu.AddItem(new GUIContent("Move Down"), false, () => EditorApplication.delayCall += () => pipeline.BuildSteps.MoveForward(step));
                        menu.AddItem(new GUIContent("Move to Bottom"), false, () => EditorApplication.delayCall += () => pipeline.BuildSteps.MoveToLast(step));
                        menu.AddSeparator(string.Empty);
                        menu.AddItem(new GUIContent("Duplicate"), false, () => EditorApplication.delayCall += () => pipeline.BuildSteps.Add(step.Duplicate()));
                        menu.AddItem(new GUIContent("Delete"), false, () => EditorApplication.delayCall += () => pipeline.BuildSteps.Remove(step));

                        menu.ShowAsContext();
                    }

                    EditorGUILayout.EndHorizontal();
                    
                    if (_foldouts[step])
                        step.OnGUI();
                }                
            }

            if (GUILayout.Button("Add Step"))
            {
                GenericMenu menu = new GenericMenu();
                foreach (string buildStepName in BuildStepUtilities.GetAllBuildSteps())
                {
                    menu.AddItem(new GUIContent(buildStepName), false, () => pipeline.BuildSteps.Add(BuildStepUtilities.CreateFromName(buildStepName)));
                }

                menu.ShowAsContext();
            }

            EditorGUILayout.Space();
            
            if (GUILayout.Button("Run Pipeline"))
            {
                bool run = EditorUtility.DisplayDialog
                (
                    "Run Pipeline",
                    "Are you sure you want to run the pipeline? Once started it cannot be stopped.",
                    "Run",
                    "Cancel"
                );

                if (run)
                    pipeline.Run();
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}