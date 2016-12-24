using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO; 

namespace CryptoBase
{
    [TestClass]
    public class Test_RC5
    {
        [TestMethod]
        public void Equal_text_long_RC5()
        {
            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            MemoryStream skey = new MemoryStream(key);
            RC5 rc5 = new RC5(skey);
            string text_long = "Imagine how it would be To be at the top Making cash money Go and tour all around the world Tell stories about all the young girls";
            string cyptext;
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(text_long));
            rc5.Encrypt(sourceDataStream, encryptedDataStream);//шифруем
            cyptext = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');
            rc5.Decrypt(encryptedDataStream, decryptedDataStream);//дешифруем
            cyptext = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(text_long, cyptext);
        }

        [TestMethod]
        public void Equal_text_block_RC5()
        {
            string text_block = "1234567890abcdef";
            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            MemoryStream skey = new MemoryStream(key);
            RC5 rc5 = new RC5(skey);
            string cyptext;
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(text_block));
            rc5.Encrypt(sourceDataStream, encryptedDataStream);//шифруем
            cyptext = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');
            rc5.Decrypt(encryptedDataStream, decryptedDataStream);//дешифруем
            cyptext = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(text_block, cyptext);
        }

        [TestMethod]
        public void Equal_text_symbol_RC5()
        {
            string text_symbol = "0";
            byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            MemoryStream skey = new MemoryStream(key);
            RC5 rc5 = new RC5(skey);
            string cyptext;
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(text_symbol));
            rc5.Encrypt(sourceDataStream, encryptedDataStream);//шифруем
            cyptext = Encoding.Default.GetString(encryptedDataStream.ToArray()).TrimEnd('\0');
            rc5.Decrypt(encryptedDataStream, decryptedDataStream);//дешифруем
            cyptext = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(text_symbol, cyptext);
        }
    }

}
