using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.BuildSources
{
    [CreateAssetMenu(fileName = "New" + nameof(BuildConfig), menuName = "Build/" + nameof(BuildConfig))]
    public class BuildConfig : ScriptableObject, IBuildSource
    {
        public string ProductName;
        public string Version;
        public string ProgramName;
        public string[] Scenes = new string[0];
        public string[] ScriptDefines = new string[0];
        public int Architecture = 1;
        public ScriptingImplementation ScriptingImplementation = ScriptingImplementation.Mono2x;
        public ManagedStrippingLevel ManagedStrippingLevel = ManagedStrippingLevel.Low;
        public BuildTarget BuildTarget = BuildTarget.StandaloneWindows64;
        public BuildOptions BuildOptions;

        private void Reset()
        {
            Scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();
        }

        public bool IsValid()
        {
            return true;
        }

        public async Task<BuildArtifact> AcquireAsync(string artifactId, CancellationToken cancellationToken)
        {
            string tempDirectory = FileUtil.GetUniqueTempPathInProject();
            
            NamedBuildTarget namedTarget = NamedBuildTarget.FromBuildTargetGroup(BuildPipeline.GetBuildTargetGroup(BuildTarget));

            int editorArchitecture = PlayerSettings.GetArchitecture(namedTarget);
            ScriptingImplementation editorScriptingImplementation = PlayerSettings.GetScriptingBackend(namedTarget);
            string editorProductName = PlayerSettings.productName;
            ManagedStrippingLevel editorManagedStrippingLevel = PlayerSettings.GetManagedStrippingLevel(namedTarget);

            try
            {
                PlayerSettings.SetArchitecture(namedTarget, Architecture);
                PlayerSettings.SetScriptingBackend(namedTarget, ScriptingImplementation);
                PlayerSettings.productName = ProductName;
                PlayerSettings.SetManagedStrippingLevel(namedTarget, ManagedStrippingLevel);

                BuildPlayerOptions options = new BuildPlayerOptions
                {
                    scenes = Scenes,
                    locationPathName = Path.Combine(tempDirectory, string.IsNullOrEmpty(ProgramName) ? BuildUtilities.GetExecutableName(ProductName, BuildTarget) : ProgramName),
                    target = BuildTarget,
                    options = BuildOptions,
                    extraScriptingDefines = ScriptDefines
                };
            
                BuildPipeline.BuildPlayer(options);

                return new BuildArtifact
                {
                    ID = artifactId,
                    RootPath = tempDirectory,
                    Files = Directory.GetFiles(tempDirectory, "*.*", SearchOption.AllDirectories),
                };
            }
            finally
            {
                PlayerSettings.SetArchitecture(namedTarget, editorArchitecture);
                PlayerSettings.SetScriptingBackend(namedTarget, editorScriptingImplementation);
                PlayerSettings.productName = editorProductName;
                PlayerSettings.SetManagedStrippingLevel(namedTarget, editorManagedStrippingLevel);
            }
        }
    }
}