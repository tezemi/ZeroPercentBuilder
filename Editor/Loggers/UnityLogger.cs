using UnityEngine;
using ZeroPercentBuilder.Attributes;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.Loggers
{
    [PipelineLogger]
    public class UnityLogger : IPipelineLogger
    {
        public void Log(string message) => Debug.Log(message);
        public void LogWarning(string message) => Debug.LogWarning(message);
        public void LogError(string message) => Debug.LogError(message);
    }
}

