using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Books_Shop.Helpers
{
    public class Hashing
    {
        public static string getHash(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            SHA256Managed hashCode = new SHA256Managed();
            byte[] hash = hashCode.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte b in hash)
            {
                hashString += String.Format("{0:x2}", b);
            }
            return hashString;
        }
    }
}