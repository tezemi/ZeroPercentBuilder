using System;

namespace ZeroPercentBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BuildStepAttribute : Attribute
    {
        public string BuildStepName { get; }

        public BuildStepAttribute(string buildStepName)
        {
            BuildStepName = buildStepName;
        }
    }
}
