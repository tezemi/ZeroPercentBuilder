using System.IO;
using System.Collections.Generic;

namespace ZeroPercentBuilder
{
    public record BuildArtifact
    {
        public bool CleanAfterPipelineRan { get; set; }
        public string ID { get; set; }
        public string RootPath { get; set; }
        public IReadOnlyList<string> Files { get; set; }

        public void CleanUp()
        {
            if (Directory.Exists(RootPath))
                Directory.Delete(RootPath, true);
        }
    }
}
