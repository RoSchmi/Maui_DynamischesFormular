using System;
using System.Collections.Generic;
using System.Text;

namespace RoSchmi.Maui.Helpers
{
    public static class AccountHelper
    {
        public static string GetActAccountFromFile(string pFolderName, string pFileName)
        {
            List<string> names = GetAccountsListFromFile(pFolderName, pFileName);

            string returnValue = names == null ? null : names.Count < 1 ? null : names[0];

            return returnValue;
        }

        public static List<string> GetAccountsListFromFile(string pFolderName, string pFileName)
        {
            string rootPath = FileSystem.Current.AppDataDirectory;
            string folderPath = Path.Combine(rootPath, pFolderName);
            string filePath = Path.Combine(folderPath, pFileName);

            List<string> returnValue = new();

            if (File.Exists(@filePath))
            {
                string fileContent = File.ReadAllText(@filePath);
                string[] accounts = fileContent.Split(',');

                var names = accounts.ToList<string>();

                if (names[0] == "")
                {
                    names.RemoveAt(0);
                }

                returnValue = names;
            }

            return returnValue;
        }

        public static void DeleteAccountsFile(string pFolderName, string pFileName)  // This is only used in tests
        {
            string rootPath = FileSystem.Current.AppDataDirectory;
            string folderPath = Path.Combine(rootPath, pFolderName);
            string filePath = Path.Combine(folderPath, pFileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var directoryInfo = new DirectoryInfo(folderPath);

            FileInfo[] fileInfos = directoryInfo.GetFiles();
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();

            if (fileInfos.Length == 0 && directoryInfos.Length == 0)
            {
                Directory.Delete(folderPath, false);
            }
        }

        /*

    private static async Task WriteListToFile(List<string> pNames, string pFolderName, string pFileName)

    string rootPath = FileSystem.Current.AppDataDirectory;
    string folderPath = Path.Combine(rootPath, pFolderName);
    string filePath = Path.Combine(folderPath, pFileName);

    string accountsString = string.Empty;
            for (int i = 0; i<pNames.Count; i++)
            {
                accountsString += pNames[i] + ',';
            }

            if (accountsString.Length > 0)
            {
                accountsString = accountsString.Remove(accountsString.Length - 1);
            }

            if (!Directory.Exists(folderPath))
    {
    Directory.CreateDirectory(folderPath);
    }

    */

        /*

        if (!(checkResult == ExistenceCheckResult.FileExists))
        {
            file = await folder.CreateFileAsync(accountsFileName,
            CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(accountsString);

        }
        else
        {
            file = await folder.CreateFileAsync(accountsFileName, CreationCollisionOption.OpenIfExists);
            await file.DeleteAsync();
            file = await folder.CreateFileAsync(accountsFileName,
            CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(accountsString);
        }
        */


        /*
    try
    {
    File.WriteAllText(filePath, accountsString);
    }
    catch (Exception ex)
    {
    await Application.Current.MainPage.DisplayAlert("Alert", "Could not store Accounts-List \r\n" + ex.Message, "OK");
    }

    }
        */

    }
}
