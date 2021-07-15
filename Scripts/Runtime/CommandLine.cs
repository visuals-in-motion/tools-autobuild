using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Visuals
{
    public class CommandLine : MonoBehaviour
    {
		public static string Run(string command)
		{
			var processInfo = new ProcessStartInfo();
			processInfo.RedirectStandardOutput = true;
			processInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.FileName = "cmd.exe";
			processInfo.WorkingDirectory = Application.dataPath.Replace("/Assets", "");
			processInfo.Arguments = "/c " + command;

			var process = Process.Start(processInfo);
			string output = process.StandardOutput.ReadToEnd();
			process.Close();

			return output;
		}
	}
}