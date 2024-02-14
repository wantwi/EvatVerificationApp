using EvatVerificationApp.DataSecurity;
using EvatVerificationApp.model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace EvatVerificationApp.helpers
{
    public static class EncryptionHelper
    {
        public static QueryDto AsJsonCast(this string stringVal)
        {
            var dataInit = stringVal.Split('&');

            var dictionary = new Dictionary<string, string> { };
            for (int i = 0; i < dataInit.Length; i++)
            {
                var equal = dataInit[i].Split('=');
                if (equal.Length == 0)
                {
                    continue;
                }
                dictionary.Add(equal[0], equal[1]);
            }
            var queryString = JsonConvert.SerializeObject(dictionary);
            return JsonConvert.DeserializeObject<QueryDto>(queryString);

        }

        public static QueryDto AsJsonData(this string stringVal)
        {
           
            return JsonConvert.DeserializeObject<QueryDto>(stringVal);

        }

        public static NG_QueryDto_1_1 AsJsonNGData(this string stringVal)
        {

            return JsonConvert.DeserializeObject<NG_QueryDto_1_1>(stringVal);

        }

        public static string DecryptStringFromBytes(string cipherText, string version)
        {
            string plaintext = null;
            if (version == "1.1")
            {
                var password = SimpleAES.Password;
                var salt = SimpleAES.Salt;
                var initial = SimpleAES.IV;

                var iv = SimpleAES.GenerateIv(initial);
                var key = SimpleAES.GetKeyFromPassword(password, salt);
                plaintext = SimpleAES.DecryptPasswordBased(cipherText, key, iv);

            }
            else if (version == "1.0")
            {
                plaintext = SimpleAES.DecryptPasswordBased128(cipherText);
            }
            else { 
            
            }


            return plaintext;
        }


      

    }


}
//NG_QueryDto_1_1