
namespace ZeroPercentBuilder.Interfaces
{
    public interface IPipelineLogger
    {
        public void Log(string message);
        public void LogWarning(string message);
        public void LogError(string message);
    }
}
