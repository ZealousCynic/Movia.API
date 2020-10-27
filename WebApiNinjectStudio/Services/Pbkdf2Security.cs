using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebApiNinjectStudio.Services
{
    public class Pbkdf2Security
    {
        private readonly byte[] _SaltBytes;
        private readonly int _NumberOfRounds;

        public Pbkdf2Security(IConfiguration configuration)
        {
            this._SaltBytes = Convert.FromBase64String(configuration["AppSettings:SaltKeyOfPbkdf2"]);
            this._NumberOfRounds = 1000;
        }

        public string HashPassword(string text)
        {
            var toBeHashed = Encoding.UTF8.GetBytes(text);
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, this._SaltBytes, this._NumberOfRounds))
            {
                return Convert.ToBase64String(rfc2898.GetBytes(32));
            }
        }
    }
}
