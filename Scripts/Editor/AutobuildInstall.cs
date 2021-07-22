
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Visuals
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    public class AutobuildInstall
    {
        private static string manifestPath;
        static AutobuildInstall()
        {
            CheckCredentials();
        }

        private static Package package;
        [SerializeField]
        private struct Package
        {
            public string name;
            public string displayName;
            public string version;
            public string unity;
            public string author;
        }

        [MenuItem("Visuals/Autobuild/Import StreamingAssets")]
        public static void CheckCredentials()
        {
            CheckDependencyInManifest();
            string streamingPath = Application.streamingAssetsPath + "/Autobuild";
            if (!File.Exists(streamingPath + "/credentials.json"))
            {
                if (!Directory.Exists(streamingPath)) Directory.CreateDirectory(streamingPath);
                File.Copy(GetPackageRelativePath() + "/Package Resources/credentials.json", streamingPath + "/credentials.json");
            }
        }

        private static string GetPackageRelativePath()
        {
            string packagePath = Path.GetFullPath("Packages/ru.visuals.autobuild");
            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                packagePath = packagePath + "/Assets/Packages/ru.visuals.autobuild";
                if (Directory.Exists(packagePath))
                {
                    return packagePath;
                }
            }

            Debug.LogError("Error: path not found");
            return null;
        }

        private static void CheckDependencyInManifest()
        {
            manifestPath = Path.GetFullPath("Packages/manifest.json");
            string googleLibrariesPackage = "    \"ru.visuals.google-libraries\": \"https://github.com/visuals-in-motion/tools-google-libraries.git\",";
            string commandPackage = "    \"ru.visuals.command\": \"https://github.com/visuals-in-motion/tools-command.git\",";
            
            List<string> file = File.ReadAllLines(manifestPath).ToList();
            
            AddDependency(file, googleLibrariesPackage);
            AddDependency(file, commandPackage);
        }

        private static void AddDependency(List<string> file, string dependency)
        {
            if(!file.Contains(dependency))
            {
                file.Insert(2, dependency);
                File.WriteAllLines(manifestPath, file);
                AssetDatabase.Refresh();
            }
        }
    }
#endif
}
