using System.Collections.Generic;

namespace ZeroPercentBuilder
{
    public record BuildArtifact
    {
        public string ID { get; set; }
        public string RootPath { get; set; }
        public IReadOnlyList<string> Files { get; set;}
    }
}
