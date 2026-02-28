using System.IO;
using System.Threading.Tasks;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.BuildSources
{
    public class ExistingBuild : IBuildSource
    {
        public string BuildDirectory;

        public Task<BuildArtifact> AcquireAsync(string artifactId)
        {
            return Task.FromResult(new BuildArtifact
            {
                CleanAfterPipelineRan = false,
                ID = artifactId,
                RootPath = BuildDirectory,
                Files = Directory.GetFiles(BuildDirectory, "*.*", SearchOption.AllDirectories),
            });
        }
    }
}
