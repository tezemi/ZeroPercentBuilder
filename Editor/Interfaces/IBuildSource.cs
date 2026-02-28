using System.Threading;
using System.Threading.Tasks;

namespace ZeroPercentBuilder.Interfaces
{
    public interface IBuildSource
    {
        Task<BuildArtifact> AcquireAsync(string artifactId);
    }
}
