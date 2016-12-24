using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CryptoBase
{
    [TestClass]
    public class TestSignatureRSA
    {
        [TestMethod]
        public void SignAndVerify()
        {
            string sourceDataString = "This message should be signed";
            MemoryStream input = new MemoryStream(Encoding.Default.GetBytes(sourceDataString));
            MemoryStream output = new MemoryStream();

            SignatureRSA rsa = new SignatureRSA();
            rsa.RSA_Params();
            rsa.SetHashFunction(new MD5());
            rsa.Sign(input, output);

            Assert.IsTrue(rsa.Verify(output));
        }
    }
}
