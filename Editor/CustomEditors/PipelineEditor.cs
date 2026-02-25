using UnityEditor;
using UnityEngine;
using ZeroPercentBuilder.BuildSteps;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.CustomEditors
{
    [CustomEditor(typeof(Pipeline))]
    public class PipelineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Pipeline pipeline = (Pipeline)target;
            
            DrawDefaultInspector();

            foreach (IBuildStep step in pipeline.BuildSteps)
            {
                step.OnGUI();
            }

            if (GUILayout.Button("Add Build Config Step"))
            {
                pipeline.BuildSteps.Add(new BuildConfigStep());
            }
            
            if (GUILayout.Button("Add Save to Disk Step"))
            {
                pipeline.BuildSteps.Add(new SaveToDiskStep());
            }
            
            if (GUILayout.Button("Clear"))
            {
                pipeline.BuildSteps.Clear();
            }
            
            if (GUILayout.Button("Build"))
            {
                pipeline.Build();
            }
        }
    }
}