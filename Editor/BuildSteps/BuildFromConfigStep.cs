using System;
using System.Threading;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.BuildSources;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    public class BuildFromConfigStep : IBuildStep
    {        
        public BuildConfig BuildConfig;
        public string BuildArtifactID;

        public async Task ExecuteAsync()
        {
            
        }

        public void OnGUI()
        {
            
        }
    }
}