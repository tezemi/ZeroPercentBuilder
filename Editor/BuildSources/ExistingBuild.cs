using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.BuildSources
{
    [CreateAssetMenu(fileName = "New" + nameof(ExistingBuild), menuName = "Build/" + nameof(ExistingBuild))]
    public class ExistingBuild : IBuildSource
    {
        public string BuildDirectory;

        public bool IsValid()
        {
            return true;
        }

        public async Task<BuildArtifact> AcquireAsync(string artifactId, CancellationToken cancellationToken)
        {
            return new BuildArtifact
            {
                ID = artifactId,
                RootPath = BuildDirectory,
                Files = Directory.GetFiles(BuildDirectory, "*.*", SearchOption.AllDirectories),
            };
        }
    }
}
