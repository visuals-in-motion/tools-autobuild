using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace Visuals
{
    #if UNITY_EDITOR
    public class UploadToDrive : EditorWindow
    {
        private string path;
        private string status = String.Empty;


        [MenuItem("Visuals/Autobuild/Upload")]
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
#endif
}
