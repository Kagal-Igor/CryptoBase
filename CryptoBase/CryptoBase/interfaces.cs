using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CryptoBase
{
    public interface iCrypto
    {
        void Encrypt(Stream input, Stream output);
        void Decrypt(Stream input, Stream output);
    }

    public interface iHash
    {
        void GetHash(Stream input, Stream output);
    }

    public interface iSign
    {
        void Sign(Stream input, Stream output);
        void SetKey(Stream key);
        void SetHashFunction(iHash hash);
        bool Verify(Stream input);
    }

}