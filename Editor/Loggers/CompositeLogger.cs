using System.Linq;
using System.Collections.Generic;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.Loggers
{
    public class CompositeLogger : IPipelineLogger
    {
        private readonly List<IPipelineLogger> _loggers;

        public void Log(string message) => _loggers.ForEach(l => l.Log(message));
        public void LogWarning(string message) => _loggers.ForEach(l => l.LogWarning(message));
        public void LogError(string message) => _loggers.ForEach(l => l.LogError(message));

        public CompositeLogger(IEnumerable<IPipelineLogger> loggers)
        {
            _loggers = loggers.ToList();
        }

        public void AddLogger(IPipelineLogger logger)
        {
            _loggers.Add(logger);
        }
    }
}
