using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Interfaces;
using ZeroPercentBuilder.Utilities;

namespace ZeroPercentBuilder.BuildSources
{
    [CreateAssetMenu(fileName = "New" + nameof(BuildConfig), menuName = "Build/" + nameof(BuildConfig))]
    public class BuildConfig : ScriptableObject, IBuildSource
    {
        public string ProductName;
        public string Version;
        public string ProgramName;
        public string[] Scenes;
        public ScriptingImplementation ScriptingImplementation;
        public BuildTarget BuildTarget;
        public BuildOptions BuildOptions;

        public bool IsValid()
        {
            return true;
        }

        public async Task<BuildArtifact> AcquireAsync(string artifactId, CancellationToken cancellationToken)
        {
            string tempDirectory = FileUtil.GetUniqueTempPathInProject();
            
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = Path.Combine(tempDirectory, string.IsNullOrEmpty(ProgramName) ? BuildUtilities.GetExecutableName(ProductName, BuildTarget) : ProgramName),
                target = BuildTarget,
                options = BuildOptions
            };
            
            BuildPipeline.BuildPlayer(options);

            return new BuildArtifact
            {
                ID = artifactId,
                RootPath = tempDirectory,
                Files = Directory.GetFiles(tempDirectory, "*.*", SearchOption.AllDirectories),
            };
        }
    }
}