using System.Threading;
using System.Threading.Tasks;

namespace ZeroPercentBuilder.Interfaces
{
    public interface IBuildSource
    {
        bool IsValid();
        Task<BuildArtifact> AcquireAsync(CancellationToken cancellationToken);
    }
}
