using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.BuildSources;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    public class SaveToDiskStep : IBuildStep
    {
        public string ArtifactID;
        public string OutputDirectory;

        public async Task ExecuteAsync(Pipeline pipeline)
        {
            BuildArtifact buildArtifact = pipeline.GetBuildArtifact(ArtifactID);
            
            foreach (string filePath in buildArtifact.Files)
            {
                File.Copy(filePath, Path.Combine(OutputDirectory, Path.GetFileName(filePath)), true);
            }
        }

        public void OnGUI()
        {
            
        }
    }
}