using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ZeroPercentBuilder.Loggers;
using ZeroPercentBuilder.Utilities;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder
{
    [CreateAssetMenu(fileName = "New" + nameof(Pipeline), menuName = "Build/" + nameof(Pipeline))]
    public class Pipeline : ScriptableObject
    {
        public bool LogToFile;
        public string LogFileDirectory;
        [SerializeReference]
        public List<IBuildStep> BuildSteps;
        public IPipelineLogger Logger { get; set; }
        public readonly List<BuildArtifact> Artifacts = new ();

        public async void Run()
        {
            // Just in case, clear this list of artifacts
            Artifacts.Clear();

            // Add all loggers to the composite logger that have the logger attribute
            // If checked, add a file logger as well
            Logger = new CompositeLogger(LoggerUtilities.GetAllLoggers());
            if (LogToFile)
            {
                ((CompositeLogger)Logger).AddLogger(new FileLogger(Path.Combine(LogFileDirectory, $"{name}_{DateTime.Now:yyyy-MM-dd_HH:mm:ss}_log.txt")));
            }
            
            bool completedWithErrors = false;
            int totalSteps = BuildSteps.Count;
            
            Logger.Log($"Running pipeline {name} with {totalSteps} steps.");
            
            try
            {
                for (int i = 0; i < totalSteps; i++)
                {
                    IBuildStep step = BuildSteps[i];
                    string stepName = BuildStepUtilities.GetBuildStepName(step);

                    EditorUtility.DisplayProgressBar("Running", stepName, (float)i / totalSteps);

                    Logger.Log($"Executing step {stepName}.");

                    await step.ExecuteAsync(this);

                    Logger.Log($"Executed step {stepName} successfully.");
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Pipeline failed:{Environment.NewLine}{e}");
                completedWithErrors = true;
            }
            finally
            {
                EditorUtility.DisplayProgressBar("Running", "Cleaning Up", 1f);

                Logger.Log($"Cleaing up artifacts.");

                foreach (BuildArtifact buildArtifact in Artifacts)
                {
                    try
                    {
                        Logger.Log($"Cleaing up artifact {buildArtifact.ID}.");

                        buildArtifact.CleanUp();
                    }
                    catch (Exception e)
                    {
                        Logger.LogError($"Failed to clean up build artifact:{Environment.NewLine}{e}");
                    }
                }

                Artifacts.Clear();

                EditorUtility.ClearProgressBar();   
            }

            if (completedWithErrors)
            {
                Logger.LogWarning($"Pipeline completed with errors.");
            }
            else
            {
                Logger.Log($"Pipeline completed successfully.");
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