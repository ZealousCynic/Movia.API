using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApiNinjectStudio.Services
{
    public class AESSecurity
    {

        private readonly string _SecretKey;

        public AESSecurity(IConfiguration configuration)
        {
            this._SecretKey = configuration["AppSettings:SecretKeyOfAes"];
        }

        public string AesEncrypt(string clearTxt)
        {
            //string secretKey = "I15TMSLO0KXUWTHO";

            var keyBytes = Encoding.UTF8.GetBytes(this._SecretKey);

            using (var cipher = new RijndaelManaged())
            {
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.KeySize = 128;
                cipher.BlockSize = 128;
                cipher.Key = keyBytes;
                cipher.IV = keyBytes;

                var valueBytes = Encoding.UTF8.GetBytes(clearTxt);

                byte[] encrypted;
                using (var encryptor = cipher.CreateEncryptor())
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = ms.ToArray();

                            var sb = new StringBuilder();
                            for (var i = 0; i < encrypted.Length; i++)
                            {
                                sb.Append(Convert.ToString(encrypted[i], 16).PadLeft(2, '0'));
                            }

                            return sb.ToString().ToUpperInvariant();
                        }
                    }
                }
            }
        }

        public string AesDecypt(string encrypted)
        {
            //string secretKey = "I15TMSLO0KXUWTHO";

            var keyBytes = Encoding.UTF8.GetBytes(this._SecretKey);

            using (var cipher = new RijndaelManaged())
            {
                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.KeySize = 128;
                cipher.BlockSize = 128;
                cipher.Key = keyBytes;
                cipher.IV = keyBytes;

                var lstBytes = new List<byte>();
                for (var i = 0; i < encrypted.Length; i += 2)
                {
                    lstBytes.Add(Convert.ToByte(encrypted.Substring(i, 2), 16));
                }

                using (var decryptor = cipher.CreateDecryptor())
                {
                    using (var msDecrypt = new MemoryStream(lstBytes.ToArray()))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
