using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Visuals
{
	public class AutoBuild
	{
		[MenuItem("Visuals/Build/Windows")]
		static void Windows()
		{
			var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
			BuildPipeline.BuildPlayer(scenes, "Build/Visuals.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
		}

#if UNITY_EDITOR_WIN
		[PostProcessBuildAttribute(1)]
		public static void OnPostBuild(BuildTarget target, string pathToBuiltProject)
		{
			string buildPath = Path.GetDirectoryName(pathToBuiltProject);
			string startPath = buildPath;

			string productName = Application.productName;
			productName = productName.Replace(Environment.NewLine, "");
			productName = productName.Replace("\n", "");

			string zipPath = $"{buildPath}\\{productName}.zip";

			if (File.Exists(zipPath))
			{
				File.Delete(zipPath);
			}

			CommandLine.Run($"powershell Compress-Archive {startPath} {zipPath}");
		}
#endif
	}
}