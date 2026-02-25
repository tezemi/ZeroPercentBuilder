using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using ZeroPercentBuilder.Attributes;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.Utilities
{
    public static class LoggerUtilities
    {
        public static List<IPipelineLogger> GetAllLoggers()
        {
            return TypeCache.GetTypesWithAttribute<PipelineLoggerAttribute>().Select(t => (IPipelineLogger)Activator.CreateInstance(t)).ToList();
        }
    }
}