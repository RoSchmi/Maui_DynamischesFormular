using Maui_DynamischesFormular.Models;

namespace Maui_DynamischesFormular.Helpers;

public static class Helper
{
    public static string GetSelectedProfileOfThisAccountFromProfilesDictionary(Dictionary<string, SuitCaseProperties> pProfilesDictionary, string pActAccount, string delimiter)
    {

        string returnValue = null;
        try
        {
            foreach (KeyValuePair<string, SuitCaseProperties> dictionaryEntry in pProfilesDictionary)
            {
                var localProperties = new SuitCaseProperties();

                if (pProfilesDictionary.TryGetValue(dictionaryEntry.Key, out localProperties))
                {
                    string extractedAccount = dictionaryEntry.Key[..dictionaryEntry.Key.IndexOf(delimiter, StringComparison.InvariantCulture)];
                    if ((localProperties.PropertiesDictionary["Selected"].Content as Maui_DynamischesFormular.Models.StringTypeContent).Value == "1" && extractedAccount == pActAccount)
                    {
                        returnValue = dictionaryEntry.Key[(dictionaryEntry.Key.IndexOf(delimiter, StringComparison.InvariantCulture) + 1)..];
                        int breakpoint = 1;
                    }
                }
            }

            return returnValue;
        }
        catch
        {
            return null;
        }
    }
}
