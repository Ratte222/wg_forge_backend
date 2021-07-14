using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
    public class Crypt
    {

        /// <summary>
        /// получить хэш  из строки
        /// </summary>
        /// <param name="value">сгенерировать хеш для этого значения</param>
        /// <returns>hash for message in Base64String</returns>
        public static string GetHashSHA512(string value)
        {
            byte[] val = Encoding.Default.GetBytes(value);
            return Convert.ToBase64String(InternalGetHashSHA512(val));

        }

        [DebuggerNonUserCode]
        private static byte[] InternalGetHashSHA512(byte[] value)
        {
            byte[] hashValue = new byte[512];
            using (SHA512 mySHA512 = SHA512.Create())
            {
                hashValue = mySHA512.ComputeHash(value);
            }
            return hashValue;
        }        
    }
}
