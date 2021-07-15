using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using UnityEditor;

namespace Visuals
{
    public class DeleteFiles
    {
        [InitializeOnLoadMethod]
        private static void Initialization()
        {
            if (CheckLibraries.Instance)
            {
                Debug.Log("true");
            }
            else
            {
                CheckLibraries.Instance = true;
                string libraryPath = Application.dataPath.Replace("Assets", "Library/PackageCache");

                string fileName = "Google.Apis.dll";

                IEnumerable<string> libraryFiles = Directory.EnumerateFiles(libraryPath, fileName, SearchOption.AllDirectories);

                foreach (string findedFile in libraryFiles)
                {
                    FileInfo FI;

                    FI = new FileInfo(findedFile);

                    Debug.Log(FI.Name + " " + FI.FullName + " ");
                }
            }
        }
    }
}