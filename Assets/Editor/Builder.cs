using UnityEditor;
using UnityEngine;
using System;
using System.IO;


public class Builder
{
	private enum BuildType
	{
		Demo,
		Show
	}


	#region Constants
	public const string OutputPathRoot = "../Builds/";
	#endregion


	#region Properties
	public static string ProjectName
	{
		get {
			string[] pathComponents = Application.dataPath.Split ('/');
			return pathComponents [pathComponents.Length - 2];
		}
	}

	public static string[] ScenePaths
	{
		get {
			string[] sceneNames = new string[EditorBuildSettings.scenes.Length];
			for (int i = 0; i < sceneNames.Length; ++i)
				sceneNames [i] = EditorBuildSettings.scenes [i].path;
			return sceneNames;
		}
	}
	#endregion


	#region Methods
	private static void PerformAndroidBuild(BuildType buildType)
	{
		string outputPath = OutputPathRoot + "Android/" + buildType.ToString() + "/";
		string outputApkFilePath = outputPath + ProjectName + "_" + buildType.ToString() + ".apk";

		if (Directory.Exists(outputPath))
			Directory.Delete(outputPath, true);

		Directory.CreateDirectory(outputPath);
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		BuildPipeline.BuildPlayer(ScenePaths, outputApkFilePath, BuildTarget.Android, BuildOptions.None);
	}

	private static void PerformiOSBuild(BuildType buildType)
	{
		// define the demo preprocessor if it's a demo
		if (buildType == BuildType.Demo)
		{
			EditorUserBuildSettings.development = true;
			EditorUserBuildSettings.allowDebugging = true;
			EditorUserBuildSettings.connectProfiler = true;
		}
		
		// determine the output path, and remove it if it already exists
		string outputPath = OutputPathRoot + "iOS/" + ProjectName + "_" + buildType.ToString() + "/";
		if (Directory.Exists(outputPath))
			Directory.Delete(outputPath, true);

		// create the new path, and begin the build
		Directory.CreateDirectory(outputPath);
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
		BuildPipeline.BuildPlayer(ScenePaths, outputPath, BuildTarget.iOS, BuildOptions.None);
	}

	public static void PerformDemoAndroidBuild()
	{
		PerformAndroidBuild(BuildType.Demo);
	}

	public static void PerformShowAndroidBuild()
	{
		PerformAndroidBuild(BuildType.Show);
	}

	public static void PerformDemoiOSBuild()
	{
		PerformiOSBuild(BuildType.Demo);
	}

	public static void PerformShowiOSBuild()
	{
		PerformiOSBuild(BuildType.Show);
	}
	#endregion
}
