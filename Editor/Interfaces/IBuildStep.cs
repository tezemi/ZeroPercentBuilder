using System.Threading;
using System.Threading.Tasks;

namespace ZeroPercentBuilder.Interfaces
{
    public interface IBuildStep
    {
        Task ExecuteAsync(Pipeline pipeline);
        void OnGUI();
        IBuildStep Duplicate();
    }
}
