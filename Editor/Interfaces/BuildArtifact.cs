using System.Collections.Generic;

namespace ZeroPercentBuilder.Interfaces
{
    public class BuildArtifact
    {
        public string ID { get; set; }
        public string RootPath { get; set; }
        public IReadOnlyList<string> Files { get; set;}

        public BuildArtifact(string rootPath, IReadOnlyList<string> files)
        {
            RootPath = rootPath;
            Files = files;
        }
    }
}
