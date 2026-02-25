using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.BuildSources;
using UnityEngine;
using UnityEditor;

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
            BuildArtifact buildArtifact = await BuildConfig.AcquireAsync(ArtifactID, CancellationToken.None);
            pipeline.StoreBuildArtifact(buildArtifact);
        }

        public void OnGUI()
        {
            ArtifactID = EditorGUILayout.TextField("Artifact ID", ArtifactID);
            BuildConfig = (BuildConfig)EditorGUILayout.ObjectField("Build Config", BuildConfig, typeof(BuildConfig));
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