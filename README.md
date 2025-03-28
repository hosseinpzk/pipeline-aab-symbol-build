# pipeline-aab-symbol-build
Unity AAB Build Pipeline

This project provides an automated pipeline to build Android App Bundles (AAB) using Unity. The pipeline leverages Azure DevOps for CI/CD and outputs the generated AAB file and symbols to the desktop.It’s designed for setups where the agent pool runs on a Windows Server.

(here are the complete detailed steps to create your Windows agent and Azure agent pool 👉 https://learn.microsoft.com/en-us/azure/devops/pipelines/agents/windows-agent?view=azure-devops)

# Features
Automated AAB build using Unity Editor.

Copies the build output (.aab) to the desktop.

Copies the generated symbols file (symbols.zip) for debugging.

Logs the build process for easy troubleshooting.


# Structure
Build Pipeline (YAML) (azure-pipeline.yaml file at the root of your repository): Configured to trigger on the main branch and run on the unity-build-h agent pool.

Build Script (C#) (BuildScript.cs at Assets/Editor/): Uses Unity’s build API to generate the AAB and handle keystore settings.

Modify the output paths or file names directly in BuildScript.cs.

Happy Building! :)
