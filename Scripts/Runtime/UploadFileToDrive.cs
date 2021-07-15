using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v3.Data.File;
using UnityEngine;

namespace Visuals
{
    public class UploadFileToDrive
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Visuals upload build";

        public static string Upload(string folderInDrive)
        {
            UserCredential credential;

            string buildPath = Application.dataPath.Replace("Assets", "Build");
            string uploadFile = Application.productName + ".zip";
            string uploadFilePath = buildPath + "/" + uploadFile;

            if (Directory.Exists(buildPath))
            {
                string credentialsPath = Application.streamingAssetsPath + "/Localization/credentials.json";

                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                string query = "mimeType!='application/vnd.google-apps.folder' and trashed = false and name = '" + uploadFile + "'" + " and '" + folderInDrive + "' in parents";
                FilesResource.ListRequest req;
                req = service.Files.List();
                req.Q = query;
                req.Fields = "files(id, name)";

                var result = req.Execute();

                if (System.IO.File.Exists(uploadFilePath))
                {
                    if (result.Files.Count > 0)
                    {
                        return UpdateFile(uploadFile, uploadFilePath, service, result.Files[0].Id);
                    }
                    else
                    {
                        return UploadFile(uploadFile, uploadFilePath, service, folderInDrive);
                    }
                }
                else
                {
                    return "Нет файла билда";
                }
            }
            else
            {
                return "Нет папки билда";
            }
        }

        private static string UpdateFile(string uploadFile, string uploadFilePath, DriveService service, string id)
        {
            try
            {
                File updatedFileMetadata = new File();
                updatedFileMetadata.Name = System.IO.Path.GetFileName(uploadFile);

                FilesResource.UpdateMediaUpload updateRequest;
                using (var stream = new FileStream(uploadFilePath, FileMode.Open))
                {
                    updateRequest = service.Files.Update(updatedFileMetadata, id, stream, GetMimeType(uploadFile));
                    updateRequest.Upload();
                    var file = updateRequest.ResponseBody;
                }
            }
            catch(IOException e)
            {
                return e.Message;
            }

            return "Файл обновлен";
        }

        private static string UploadFile(string uploadFile, string uploadFilePath, DriveService service, string folderInDrive)
        {
            try
            {
                var driveFile = new File();
                driveFile.Name = System.IO.Path.GetFileName(uploadFile);
                driveFile.MimeType = GetMimeType(uploadFile);
                driveFile.Parents = new string[] { folderInDrive };


                using (var stream = new FileStream(uploadFilePath, FileMode.Open))
                {
                    var request = service.Files.Create(driveFile, stream, GetMimeType(uploadFile));
                    request.Fields = "id";

                    var response = request.Upload();
                }
            }
            catch (IOException e)
            {
                return e.Message;
            }

            return "Файл загружен";
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}