using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CryptoBase
{
    [TestClass]
    public class TestMD5
    {
        [TestMethod]
        public void EmptyHashEquals()
        {
            string sourceDataString = "";
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream output = new MemoryStream();
            MD5 md5 = new MD5();
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            md5.GetHash(input, output);

            string myHash = Encoding.Default.GetString(output.ToArray()).TrimEnd('\0');
            Assert.AreEqual(comparer.Compare(myHash, "d41d8cd98f00b204e9800998ecf8427e"), 0);
        }

        [TestMethod]
        public void HashEquals()
        {
            string sourceDataString = "Imagine how it would be To be at the top Making cash money Go and tour all around the world Tell stories about all the young girls";

            MD5 md5 = new MD5();
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream output = new MemoryStream();

            md5.GetHash(input, output);
            string myHash = Encoding.Default.GetString(output.ToArray()).TrimEnd('\0');

            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(sourceDataString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string hash = sBuilder.ToString();

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            Assert.AreEqual(comparer.Compare(myHash, hash), 0);

        }
    }
}
