using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.BuildSources;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    public class BuildConfigStep : IBuildStep
    {
        public string ArtifactId;
        public BuildConfig BuildConfig;

        public async Task ExecuteAsync(Pipeline pipeline)
        {
            BuildArtifact buildArtifact = await BuildConfig.AcquireAsync(ArtifactId, CancellationToken.None);
            pipeline.StoreBuildArtifact(buildArtifact);
        }

        public void OnGUI()
        {
            
        }
    }
}