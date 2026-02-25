using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.BuildSources;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.Attributes;

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
            pipeline.Logger.Log($"Saving build artifact with ID {ArtifactID} to {OutputDirectory}.");

            BuildArtifact buildArtifact = pipeline.GetBuildArtifact(ArtifactID);
            
            foreach (string filePath in buildArtifact.Files)
            {
                string relativePath = Path.GetRelativePath(buildArtifact.RootPath, filePath);
                string output = Path.Combine(OutputDirectory, relativePath);

                pipeline.Logger.Log($"Copying {filePath} to {output}.");

                string directory = Path.GetDirectoryName(output);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);

                File.Copy(filePath, output, true);
            }

            pipeline.Logger.Log($"Saved build artifact to {OutputDirectory} successfully.");
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