using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.Attributes;

namespace ZeroPercentBuilder.Utilities
{
    public static class BuildStepUtilities
    {
        public static string GetBuildStepName(IBuildStep buildStep)
        {
            Type buildStepType = buildStep.GetType();
            BuildStepAttribute attribute = Attribute.GetCustomAttribute(buildStepType, typeof(BuildStepAttribute)) as BuildStepAttribute;

            return attribute != null ? attribute.BuildStepName : buildStepType.Name;
        }

        public static string GetBuildStepName(Type buildStepType)
        {
            BuildStepAttribute attribute = Attribute.GetCustomAttribute(buildStepType, typeof(BuildStepAttribute)) as BuildStepAttribute;

            return attribute != null ? attribute.BuildStepName : buildStepType.Name;
        }

        public static IEnumerable<string> GetAllBuildSteps()
        {
            return TypeCache
                .GetTypesWithAttribute<BuildStepAttribute>()
                .Select(GetBuildStepName);
        }

        public static IBuildStep CreateFromName(string name)
        {
            TypeCache.TypeCollection buildStepTypes = TypeCache.GetTypesWithAttribute<BuildStepAttribute>();
            foreach (Type buildStepType in buildStepTypes)
            {
                if (GetBuildStepName(buildStepType) == name)
                {
                    return (IBuildStep)Activator.CreateInstance(buildStepType);
                }
            }

            throw new ArgumentException($"{name} does not match any build step.", nameof(name));
        }
    }
}

