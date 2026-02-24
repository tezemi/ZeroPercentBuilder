using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.BuildSources
{
    [CreateAssetMenu(fileName = "New" + nameof(BuildConfig), menuName = "Build/" + nameof(BuildConfig))]
    public class BuildConfig : ScriptableObject, IBuildSource
    {
        public string ProductName;  // leave blank for default
        public string Version;      // leave blank for default
        public string ProgramName;
        public string[] Scenes;
        public ScriptingImplementation ScriptingImplementation;
        public BuildTarget BuildTarget;
        public BuildOptions BuildOptions;

        public bool IsValid()
        {
            return true;
        }

        public async Task<BuildArtifact> AcquireAsync(CancellationToken cancellationToken)
        {
            string tempDirectory = FileUtil.GetUniqueTempPathInProject();
            BuildPipeline.BuildPlayer(Scenes, $"{tempDirectory}/{ProgramName}", BuildTarget, BuildOptions);

            return new BuildArtifact(tempDirectory, Directory.GetFiles(tempDirectory, "*.*", SearchOption.AllDirectories));
        }
    }
}