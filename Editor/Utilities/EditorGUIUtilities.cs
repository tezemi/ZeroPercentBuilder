using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ZeroPercentBuilder.Utilities
{
    public static class EditorGUIUtilities
    {
        private static readonly Dictionary<string, ReorderableList> SceneLists = new();
        
        private static readonly GUIStyle PlaceholderStyle = new (EditorStyles.textField)
        {
            fontStyle = FontStyle.Italic
        };

        private static readonly string[] ArchitectureOptions = { "x86", "x86_64", "ARM64" };
        private static readonly int[] ArchitectureValues = { 0, 1, 2 };
        
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
        
        public static string FilePicker(string label, string current, Action<string> onSelected)
        {            
            EditorGUILayout.BeginHorizontal();

            string result = EditorGUILayout.TextField(label, current);
            string output = result;
            
            if (GUILayout.Button("…", GUILayout.Width(30f)))
            {
                var captured = result;
                EditorApplication.delayCall += () =>
                {
                    var path = EditorUtility.OpenFilePanel(label, captured, string.Empty);
                    if (!string.IsNullOrEmpty(path))
                        onSelected(path);
                };
            }

            EditorGUILayout.EndHorizontal();

            return output;
        }
        
        public static string TextFieldWithPlaceHolder(string label, string value, string placeholder, params GUILayoutOption[] options)
        {
            string result = EditorGUILayout.TextField(label, value, options);

            if (string.IsNullOrEmpty(value))
            {
                var lastRect = GUILayoutUtility.GetLastRect();
            
                PlaceholderStyle.normal.textColor = new Color
                (
                    EditorStyles.textField.normal.textColor.r,
                    EditorStyles.textField.normal.textColor.g,
                    EditorStyles.textField.normal.textColor.b,
                    0.4f
                );
            
                EditorGUI.LabelField(lastRect, label, placeholder, PlaceholderStyle);
            }

            return result;
        }
        
        public static void StringArray(string label, ref string[] items)
        {
            EditorGUILayout.LabelField(label);

            for (int i = 0; i < items.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                items[i] = EditorGUILayout.TextField(items[i]);
                if (GUILayout.Button("-", GUILayout.Width(25f)))
                    items = items.Where((_, index) => index != i).ToArray();

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+", GUILayout.Width(25f)))
                items = items.Append(string.Empty).ToArray();
        }

        public static int ArchitecturePopup(string label, int currentArchitecture)
        {
            return EditorGUILayout.IntPopup
            (
                label,
                currentArchitecture,
                ArchitectureOptions,
                ArchitectureValues
            );
        }

        public static string[] SceneList(string label, string[] scenes)
        {
            ReorderableList reorderableList = GetOrCreateList(label, new List<string>(scenes));
            
            if (!string.IsNullOrEmpty(label))
                EditorGUILayout.LabelField(label);
            
            reorderableList.list = new List<string>(scenes);
            reorderableList.DoLayoutList();
            
            return ((List<string>)reorderableList.list).ToArray();
        }
        
        private static ReorderableList GetOrCreateList(string key, List<string> scenes)
        {
            if (SceneLists.TryGetValue(key, out var existing))
                return existing;

            ReorderableList reorderableList = new ReorderableList
            (
                scenes,
                typeof(string),
                true,
                false,
                true,
                true
            );

            reorderableList.drawElementCallback = (rect, index, _, _) =>
            {
                List<string> paths = (List<string>)reorderableList.list;
                SceneAsset current = AssetDatabase.LoadAssetAtPath<SceneAsset>(paths[index]);

                EditorGUI.BeginChangeCheck();
                
                SceneAsset picked = EditorGUI.ObjectField
                (
                    new Rect(rect.x, rect.y + 1, rect.width, EditorGUIUtility.singleLineHeight),
                    current, 
                    typeof(SceneAsset), 
                    false
                ) as SceneAsset;

                if (EditorGUI.EndChangeCheck())
                    paths[index] = picked != null ? AssetDatabase.GetAssetPath(picked) : string.Empty;
            };

            reorderableList.onAddCallback = l => ((List<string>)l.list).Add(string.Empty);
            reorderableList.elementHeight  = EditorGUIUtility.singleLineHeight + 4;

            SceneLists[key] = reorderableList;
            
            return reorderableList;
        }
    }
}


