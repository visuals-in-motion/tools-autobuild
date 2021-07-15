using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using XDiffGui;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;


namespace Visuals
{
    public class UploadToDrive : EditorWindow
    {
        private string path;
        private string status = String.Empty;

        [MenuItem("Visuals/Build/Upload")]
        public static void ShowWindow()
        {
            UploadToDrive window = GetWindow<UploadToDrive>();
            window.titleContent = new GUIContent("Загрузка на гугл диск");
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 20), "ссылка на диск");
            GUI.Label(new Rect(120, 30, 400, 20), status);

            path = GUI.TextField(new Rect(120, 10, 400, 20), path);

            if (GUI.Button(new Rect(10, 30, 100, 25), "Загрузить"))
            {
                if (String.IsNullOrEmpty(path))
                {
                    status = "Укажите ссылку на гугл диск";
                }
                else
                    status = UploadFileToDrive.Upload(ParseFolderLinq(path));
            }
        }

        private string ParseFolderLinq(string linq)
        {
            string sharingPart = "?usp=sharing";
            int index = linq.IndexOf(sharingPart, 0);

            if (index != -1)
            {
                linq = linq.Remove(index, sharingPart.Length);
            }

            string foldersPart = "folders/";
            int indexOfFolders = linq.IndexOf(foldersPart, 0);

            if (indexOfFolders != -1)
            {
                linq = linq.Remove(0, indexOfFolders + foldersPart.Length);
            }

            return linq;
        }
    }
}

