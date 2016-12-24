using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CryptoBase
{
    [TestClass]
    public class TestElgamal
    {
        [TestMethod]
        public void EncryptDecriptEqualsElGamal()
        {
            byte[] key = { 251, 148, 6 };
            MemoryStream skey = new MemoryStream(key);

            string sourceDatastring = "something";
            string decryptedDatastring;

            Elgamal elGamal = new Elgamal(skey);

            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDatastring));
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            elGamal.Encrypt(sourceDataStream, encryptedDataStream);
            elGamal.Decrypt(encryptedDataStream, decryptedDataStream);

            decryptedDatastring = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDatastring, decryptedDatastring);
        }
        
        [TestMethod]
        public void EncryptDecryptDifferentLengthElGamal()
        {
            byte[] key = { 251, 148, 6 };
            MemoryStream skey = new MemoryStream(key);

            string sourceDatastringLong = "Imagine how it would be To be at the top Making cash money Go and tour all around the world Tell stories about all the young girls";//to encrypt by ElGamal algorithm";
            string sourceDatastringShort = "1234567890abcdef";
            string sourceDatastringSymbol = "z";
            string decryptedDatastring;

            Elgamal elgamal = new Elgamal(skey);

            //Long
            MemoryStream sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDatastringLong));
            MemoryStream encryptedDataStream = new MemoryStream();
            MemoryStream decryptedDataStream = new MemoryStream();

            elgamal.Encrypt(sourceDataStream, encryptedDataStream);
            elgamal.Decrypt(encryptedDataStream, decryptedDataStream);

            decryptedDatastring = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDatastringLong, decryptedDatastring);

            //Short
            sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDatastringShort));
            decryptedDataStream = new MemoryStream();
            encryptedDataStream = new MemoryStream();

            elgamal.Encrypt(sourceDataStream, encryptedDataStream);
            elgamal.Decrypt(encryptedDataStream, decryptedDataStream);

            decryptedDatastring = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDatastringShort, decryptedDatastring);

            //Symbol
            sourceDataStream = new MemoryStream(Encoding.Default.GetBytes(sourceDatastringSymbol));
            decryptedDataStream = new MemoryStream();
            encryptedDataStream = new MemoryStream();

            elgamal.Encrypt(sourceDataStream, encryptedDataStream);
            elgamal.Decrypt(encryptedDataStream, decryptedDataStream);

            decryptedDatastring = Encoding.Default.GetString(decryptedDataStream.ToArray()).TrimEnd('\0');
            Assert.AreEqual(sourceDatastringSymbol, decryptedDatastring);
        }
         
    }
}
