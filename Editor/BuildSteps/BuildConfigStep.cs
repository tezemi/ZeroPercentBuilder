using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.Attributes;
using ZeroPercentBuilder.BuildSources;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    [BuildStep("Build Game from Build Config")]
    public class BuildConfigStep : IBuildStep
    {
        public string ArtifactID;
        public BuildConfig BuildConfig;

        public async Task ExecuteAsync(Pipeline pipeline)
        {
            pipeline.Logger.Log($"Creating build artifact for {BuildConfig.name}");

            BuildArtifact buildArtifact = await BuildConfig.AcquireAsync(ArtifactID);
            pipeline.StoreBuildArtifact(buildArtifact);

            pipeline.Logger.Log($"Stored build artifact with ID {ArtifactID}.");
        }

        public void OnGUI()
        {
            ArtifactID = EditorGUILayout.TextField("Artifact ID", ArtifactID);
            BuildConfig = (BuildConfig)EditorGUILayout.ObjectField("Build Config", BuildConfig, typeof(BuildConfig), false);
        }

        public IBuildStep Duplicate()
        {
            return new BuildConfigStep
            {
                ArtifactID = ArtifactID,
                BuildConfig = BuildConfig
            };
        }
    }
}