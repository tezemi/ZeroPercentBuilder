using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder
{
    [CreateAssetMenu(fileName = "New" + nameof(Pipeline), menuName = "Build/" + nameof(Pipeline))]
    public class Pipeline : ScriptableObject
    {
        [SerializeReference]
        public List<IBuildStep> BuildSteps;
        public readonly List<BuildArtifact> Artifacts = new ();

        public async void Build()
        {
            Artifacts.Clear();
            
            foreach (IBuildStep step in BuildSteps)
            {
                await step.ExecuteAsync(this);
            }
        }

        public void StoreBuildArtifact(BuildArtifact artifact)
        {
            Artifacts.Add(artifact);
        }

        public BuildArtifact GetBuildArtifact(string buildArtifactID)
        {
            return Artifacts.First(b => b.ID == buildArtifactID);
        }
    }
}