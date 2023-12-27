using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.Utils
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            for (var i = 0; i < 32; i++)
                bytes = SHA256.HashData(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
