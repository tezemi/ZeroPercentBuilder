using System.Threading;
using System.Threading.Tasks;

namespace ZeroPercentBuilder.Interfaces
{
    public interface IBuildTarget
    {        
        Task<bool> ValidateAsync(CancellationToken cancellationToken);
        Task DeployAsync(CancellationToken cancellationToken);        
    }
}
