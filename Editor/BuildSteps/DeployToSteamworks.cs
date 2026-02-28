using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Attributes;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.BuildSteps
{
    [Serializable]
    [BuildStep("Deploy to Steamworks")]
    public class DeployToSteamworks : IBuildStep
    {
        public string AppID;
        public string Branch;
        public SteamDepot[] Depots = Array.Empty<SteamDepot>();
        
        public async Task ExecuteAsync(Pipeline pipeline)
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "Steam_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);

            try
            {
                foreach (SteamDepot depot in Depots)
                {
                    BuildArtifact artifact = pipeline.GetBuildArtifact(depot.ArtifactID);
                    string depotVDFPath = Path.Combine(tempDirectory, $"depot_{depot.DepotID}.vdf");
                    await File.WriteAllTextAsync(depotVDFPath, GenerateDepotVDF(depot, artifact.RootPath));
                }

                string appVDFPath = Path.Combine(tempDirectory, "app_build.vdf");
                await File.WriteAllTextAsync(appVDFPath, GenerateAppVDF(tempDirectory, Depots));

                await RunSteamCMD(appVDFPath, pipeline);
            }
            finally
            {
                Directory.Delete(tempDirectory, true);
            }
        }

        public void OnGUI()
        {
            AppID = EditorGUILayout.TextField("App ID", AppID);
            Branch = EditorGUILayout.TextField("Branch", Branch);

            EditorGUILayout.LabelField("Depots");
            
            for (int i = 0; i < Depots.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                
                EditorGUILayout.Foldout
                (
                    true, 
                    $"Depot {i + 1}",
                    true,
                    EditorStyles.foldoutHeader
                );

                if (GUILayout.Button("-", GUILayout.Width(25f)))
                {
                    Depots = Depots.Where((_, index) => index != i).ToArray();
                    break;
                }
                
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                
                EditorGUILayout.BeginVertical();

                Depots[i].DepotID = EditorGUILayout.TextField("Depot ID", Depots[i].DepotID);
                Depots[i].ArtifactID = EditorGUILayout.TextField("Artifact ID", Depots[i].ArtifactID);

                EditorGUILayout.EndVertical();

                EditorGUI.indentLevel--;
                
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("+", GUILayout.Width(25f)))
                Depots = Depots.Append(new SteamDepot()).ToArray();
        }

        public IBuildStep Duplicate()
        {
            return new DeployToSteamworks
            {
                AppID = AppID,
                Branch = Branch,
                Depots = Depots.ToArray()
            };
        }
        
        private async Task RunSteamCMD(string appVDFPath, Pipeline pipeline)
        {
            string steamCMDPath = PipelinePreferences.SteamCMDPath;
            string username = PipelinePreferences.SteamUsername;

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = steamCMDPath,
                    Arguments = $"+login {username} +run_app_build \"{appVDFPath}\" +quit",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            process.OutputDataReceived += (_, e) => { if (e.Data != null) pipeline.Logger.Log(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data != null) pipeline.Logger.LogError(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await Task.Run(() => process.WaitForExit());

            if (process.ExitCode != 0)
                throw new Exception($"SteamCMD failed with exit code {process.ExitCode}.");
        }
        
        private string GenerateDepotVDF(SteamDepot depot, string contentRoot)
        {
            return
                "\"DepotBuild\"\n" +
                "{\n" +
                $"\t\"DepotID\" \"{depot.DepotID}\"\n" +
                $"\t\"ContentRoot\" \"{contentRoot}\"\n" +
                "\t\"FileMapping\"\n" +
                "\t{\n" +
                "\t\t\"LocalPath\" \"*\"\n" +
                "\t\t\"DepotPath\" \".\"\n" +
                "\t\t\"recursive\" \"1\"\n" +
                "\t}\n" +
                "}";
        }
        
        private string GenerateAppVDF(string tempDir, SteamDepot[] depots)
        {
            var depotLines = new StringBuilder();
            foreach (var depot in depots)
                depotLines.AppendLine("\t\t\"" + depot.DepotID + "\" \"depot_" + depot.DepotID + ".vdf\"");

            return
                "\"AppBuild\"\n" +
                "{\n" +
                "\t\"AppID\" \"" + AppID + "\"\n" +
                "\t\"Desc\" \"" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss") + "\"\n" +
                "\t\"BuildOutput\" \"" + tempDir + "/logs\"\n" +
                "\t\"SetLive\" \"" + Branch + "\"\n" +
                "\t\"Depots\"\n" +
                "\t{\n" +
                depotLines +
                "\t}\n" +
                "}";
        }
        
        [Serializable]
        public struct SteamDepot
        {
            public string DepotID;
            public string ArtifactID;
        }
    }
}