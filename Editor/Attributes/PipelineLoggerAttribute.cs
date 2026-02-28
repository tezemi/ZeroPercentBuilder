using System;
using JetBrains.Annotations;

namespace ZeroPercentBuilder.Attributes
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Class)]
    public class PipelineLoggerAttribute : Attribute
    {
        // ...
    }
}
