using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.BuildSources;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Utilities;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    [BuildStep("Save Build to Disk")]
    public class SaveToDiskStep : IBuildStep
    {
        public string ArtifactID;
        public string OutputDirectory;

        public async Task ExecuteAsync(Pipeline pipeline)
        {
            BuildArtifact buildArtifact = pipeline.GetBuildArtifact(ArtifactID);
            
            foreach (string filePath in buildArtifact.Files)
            {
                File.Copy(filePath, Path.Combine(OutputDirectory, filePath.Replace(buildArtifact.RootPath, string.Empty)), true);
            }
        }

        public void OnGUI()
        {
            ArtifactID = EditorGUILayout.TextField("Artifact ID", ArtifactID);
            OutputDirectory = EditorGUIUtilities.FolderPicker("Output Directory", OutputDirectory, s => OutputDirectory = s);
        }

        public IBuildStep Duplicate()
        {
            return new SaveToDiskStep
            {
                ArtifactID = ArtifactID,
                OutputDirectory = OutputDirectory
            };
        }
    }
}