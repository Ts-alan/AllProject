using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace VirusBlokAda.CC.Common
{
    /// <summary>
    /// Сервисный класс, предоставляет различные методы для обслуживания
    /// бизнес-логики
    /// </summary>
    public static class Anchor
    {
        /// <summary>
        /// Convert dump string to string
        /// </summary>
        /// <param name="data">Dump byte array</param>
        /// <returns>Result string</returns>
        public static String ConvertToDumpString(Byte[] data)
        {
            String str = String.Empty;
            foreach (Byte b in data)
            {
                str += b.ToString("X2");
            }
            return str;
        }

        /// <summary>
        /// Get string from Base64
        /// </summary>
        /// <param name="source">Base64 string</param>
        /// <returns>Result string</returns>
        public static String FromBase64String(String source)
        {
            Byte[] bs = Convert.FromBase64String(source);
            return Encoding.Unicode.GetString(bs);
        }

        public static String ConvertFromBase64(String value, Int32 encoded)
        {
            if (encoded == 1)
            {
                return FromBase64String(value);
            }
            else
                return value;
        }

        public static String ToBase64String(String source)
        {
            Byte[] bs = Encoding.Unicode.GetBytes(source);
            return Convert.ToBase64String(bs);
        }


        /// <summary>
        /// Get Md5 for string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Md5 string</returns>
        public static String GetMd5Hash(String input)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Create a new instance of the MD5CryptoServiceProvider object.
            using (MD5 md5Hasher = MD5.Create())
            {

                // Convert the input string to a byte array and compute the hash.
                Byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (Int32 i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Get Md5 for stream
        /// </summary>
        /// <param name="input">Input stream</param>
        /// <returns>Md5 string</returns>
        public static String GetMd5Hash(Stream input)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Create a new instance of the MD5CryptoServiceProvider object.
            using (MD5 md5Hasher = MD5.Create())
            {

                // Compute the hash.
                Byte[] data = md5Hasher.ComputeHash(input);

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (Int32 i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }      
    }
}