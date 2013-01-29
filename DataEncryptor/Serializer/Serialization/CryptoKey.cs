using System;
using System.Security.Cryptography;

namespace DataEncryptor.Serialization
{
    public class CryptoKey
    {
        private const int Iterations = 1000;
        private const int PasswordLength = 24;

        public byte[] KeyBytes { get; private set; }

        public byte[] IVBytes
        {
            get
            {
                return new byte[] { 15, 199, 56, 77, 244, 126, 107, 239 };
            }
        }

        public byte[] SaltBytes
        {
            get
            {
                return new byte[] { 124, 57, 98, 77, 43, 21, 76, 13 };
            }
        }

        private byte[] DeriveBytes(string password)
        {
            using (var rfc = new Rfc2898DeriveBytes(password, SaltBytes, 1000))
            {
                return rfc.GetBytes(PasswordLength);
            }
        }

        public CryptoKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.CryptographyKeyNull);
            }
            this.KeyBytes = DeriveBytes(key);
        }
    }
}