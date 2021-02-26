using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Protector
{
    public class Crypt
    {
        public static string Encode(string plaintext)
        {
                var sha = new SHA1Managed();
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
	            return Convert.ToBase64String(hash);
        }
    }
}
