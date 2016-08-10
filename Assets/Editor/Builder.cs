using UnityEditor;
using UnityEngine;
using System;
using System.IO;


public class Builder
{
	private enum PlatformType
	{
		Android = BuildTarget.Android,
		iOS = BuildTarget.iOS
	}


	private enum BuildType
	{
		Demo,
		Show
	}


	#region Constants
	public const string OutputPathRoot = "../Builds/";
	public const string BundlePrefix = "com.pokergamesinteractive.";
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
		// determine the output path, and remove it if it already exists
		string outputPath = OutputPathRoot + "iOS/" + ProjectName + "_" + buildType.ToString() + "/";
		if (Directory.Exists(outputPath))
			Directory.Delete(outputPath, true);

		// create the new path, and begin the build
		Directory.CreateDirectory(outputPath);
		BuildPipeline.BuildPlayer(ScenePaths, outputPath, BuildTarget.iOS, BuildOptions.None);
	}

	private static void PerformBuild(PlatformType platform, BuildType buildType)
	{
		var bundleSuffix = "_show";
		
		// define the demo preprocessor if it's a demo
		EditorUserBuildSettings.SwitchActiveBuildTarget((BuildTarget)buildType);
		if (buildType == BuildType.Demo)
		{
			EditorUserBuildSettings.development = true;
			EditorUserBuildSettings.allowDebugging = true;
			EditorUserBuildSettings.connectProfiler = true;
			bundleSuffix = "_demo";
		}
		PlayerSettings.bundleIdentifier = BundlePrefix + ProjectName.ToLower() + bundleSuffix;

		// build for the respective platform
		switch (platform)
		{
		case PlatformType.Android:
			PerformAndroidBuild(buildType);
			break;

		case PlatformType.iOS:
			PerformiOSBuild(buildType);
			break;
		}
	}

	public static void PerformDemoAndroidBuild()
	{
		PerformBuild(PlatformType.Android, BuildType.Demo);
	}

	public static void PerformShowAndroidBuild()
	{
		PerformBuild(PlatformType.Android, BuildType.Show);
	}

	public static void PerformDemoiOSBuild()
	{
		PerformBuild(PlatformType.iOS, BuildType.Demo);
	}

	public static void PerformShowiOSBuild()
	{
		PerformBuild(PlatformType.iOS, BuildType.Show);
	}
	#endregion
}
