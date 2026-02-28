using System;
using System.Threading.Tasks;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.Attributes;
using ZeroPercentBuilder.BuildSources;
using UnityEditor;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    [BuildStep("Acquire Existing Build on Disk")]
    public class ExistingBuildStep : IBuildStep
    {
        public string ArtifactID;
        public string BuildDirectory;

        public async Task ExecuteAsync(Pipeline pipeline)
        {
            pipeline.Logger.Log($"Acquiring build artifact '{ArtifactID}' from '{BuildDirectory}'.");

            ExistingBuild existingBuild = new ExistingBuild();
            existingBuild.BuildDirectory = BuildDirectory;

            BuildArtifact buildArtifact = await existingBuild.AcquireAsync(ArtifactID);
            pipeline.StoreBuildArtifact(buildArtifact);

            pipeline.Logger.Log($"Acquired build artifact '{ArtifactID}' from '{BuildDirectory}' successfully.");
        }

        public void OnGUI()
        {
            ArtifactID = EditorGUILayout.TextField("Artifact ID", ArtifactID);
            BuildDirectory = EditorGUIUtilities.FolderPicker("Build Directory", BuildDirectory, s => BuildDirectory = s);
        }

        public IBuildStep Duplicate()
        {
            return new ExistingBuildStep
            {
                ArtifactID = ArtifactID,
                BuildDirectory = BuildDirectory
            };
        }
    }
}
