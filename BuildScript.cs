// Create the BuildScript.cs file in /Assets/Editor/ and write this code in it.

using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using System.Linq;

public class BuildScript
{
    [MenuItem("Build/Build AAB")]
    public static void BuildAAB()
    {
        // Configure the keystore for signing the AAB file
        PlayerSettings.Android.useCustomKeystore = true; //IF YOU'R USING DEBUGMODE, CHANGE IT TO FALSE AND COMMENT NEXT 3 LINES
        PlayerSettings.Android.keystoreName = @"YOUR KEYSTORE FILE PATH C:\...";
        PlayerSettings.Android.keystorePass = "KEYSTOREPASS";
        PlayerSettings.Android.keyaliasName = "KEYALIASNAME";
        PlayerSettings.Android.keyaliasPass = "KEYALIASPASS";

        // Enable symbol generation for debugging
        EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;

        // Get the list of active scenes for building
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
      
        string buildFolder = @"C:\agent\_work\1\a\";
        string aabName = "OUTPUT-NAME.aab";  // Output file name
        string fullPath = Path.Combine(buildFolder, aabName);

        // Ensure the output directory exists
        if (!Directory.Exists(buildFolder))
        {
            Directory.CreateDirectory(buildFolder);
        }

        // Check if any scenes are configured for the build
        if (scenes.Length == 0)
        {
            Debug.LogError("No scenes found in Build Settings!");
            return;
        }

        // Enable building as an Android App Bundle (AAB)
        EditorUserBuildSettings.buildAppBundle = true;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = fullPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // Start the build process
        Debug.Log("Starting AAB Build...");
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        // Check the build result
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("AAB Build Completed Successfully: " + fullPath);

            // Copy the AAB file to the desktop
            string desktopAabPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "OutPut-Name.aab");
            File.Copy(fullPath, desktopAabPath, true);
            Debug.Log($"AAB copied to: {desktopAabPath}");

            // Find and copy the symbols file to the desktop
            string symbolsPath = Path.Combine(buildFolder, "symbols.zip");
            string desktopSymbolsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "symbols.zip");

            if (File.Exists(symbolsPath))
            {
                File.Copy(symbolsPath, desktopSymbolsPath, true);
                Debug.Log($"Symbols file copied to: {desktopSymbolsPath}");
            }
            else
            {
                Debug.LogWarning("Symbols file not found! Maybe Unity didn't generate it.");
            }
        }
        else
        {
            Debug.LogError("Build Failed!");
            Debug.LogError($"Errors: {summary.totalErrors}");
            Debug.LogError($"Warnings: {summary.totalWarnings}");
        }
    }
}
