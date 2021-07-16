
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Visuals
{
    #if UNITY_EDITOR
    [InitializeOnLoad]
    public class AutobuildInstall
    {
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

    }
#endif
}
