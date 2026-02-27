using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.Attributes;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    [BuildStep("Save Build to Disk")]
    public class SaveToDiskStep : IBuildStep
    {
        public bool CleanBuild;
        public string ArtifactID;
        public string OutputDirectory;

        public async Task ExecuteAsync(Pipeline pipeline)
        {
            BuildArtifact buildArtifact = pipeline.GetBuildArtifact(ArtifactID);
            
            if (CleanBuild)
            {
                pipeline.Logger.Log($"Cleaning contents of {OutputDirectory}.");

                if (IsSafeBuildPath(OutputDirectory))
                {
                    Directory.Delete(OutputDirectory, true);
                    Directory.CreateDirectory(OutputDirectory);
                }
                else
                {
                    pipeline.Logger.LogWarning($"Did not clean {OutputDirectory}. For safety reasons, only folders inside of the game's project folder can be cleaned.");
                }
            }
            
            pipeline.Logger.Log($"Saving build artifact with ID {ArtifactID} to {OutputDirectory}.");
            
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
            bool wasCleanBuild = CleanBuild;
            
            CleanBuild = EditorGUILayout.Toggle("Clean Build", CleanBuild);
            ArtifactID = EditorGUILayout.TextField("Artifact ID", ArtifactID);
            OutputDirectory = EditorGUIUtilities.FolderPicker("Output Directory", OutputDirectory, s => OutputDirectory = s);

            if (!wasCleanBuild && CleanBuild)
            {
                CleanBuild = EditorUtility.DisplayDialog
                (
                    "Clean Build",
                    $"When this step is executed, it will delete all files in:{Environment.NewLine}{OutputDirectory}{Environment.NewLine}Are you sure?",
                    "Confirm", 
                    "Cancel"
                );
            }
        }

        public IBuildStep Duplicate()
        {
            return new SaveToDiskStep
            {
                ArtifactID = ArtifactID,
                OutputDirectory = OutputDirectory
            };
        }
        
        private bool IsSafeBuildPath(string path)
        {
            string fullOutput  = Path.GetFullPath(path);
            string projectRoot = Path.GetFullPath(Application.dataPath + "/../");
            
            return fullOutput.StartsWith(projectRoot);
        }
    }
}