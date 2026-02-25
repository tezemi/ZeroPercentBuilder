using System;
using UnityEngine;
using UnityEditor;

namespace ZeroPercentBuilder.Utilities
{
    public static class EditorGUIUtilities
    {
        public static string FolderPicker(string label, string current, Action<string> onSelected)
        {            
            EditorGUILayout.BeginHorizontal();

            string result = EditorGUILayout.TextField(label, current);
            string output = result;
            
            if (GUILayout.Button("…", GUILayout.Width(30f)))
            {
                var captured = result;
                EditorApplication.delayCall += () =>
                {
                    var path = EditorUtility.OpenFolderPanel(label, captured, string.Empty);
                    if (!string.IsNullOrEmpty(path))
                        onSelected(path);
                };
            }

            EditorGUILayout.EndHorizontal();

            return output;
        }
    }
}


