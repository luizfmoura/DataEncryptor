using System;
using System.Security.Cryptography;
using System.Text;

namespace DataEncryptor.Serialization
{
    public class CryptoKey
    {
        private const int Iterations = 1000;
        private const int PasswordLength = 32;
        private const int IVLength = 16;

        public byte[] KeyBytes { get; private set; }

        public byte[] IVBytes { get; private set; }

        public byte[] SaltBytes { get; private set; }

        private byte[] DeriveBytes(string password, byte[] salt, int length)
        {
            using (var rfc = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                return rfc.GetBytes(length);
            }
        }

        public CryptoKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.CryptographyKeyNull);
            }
            var keyBytes = Encoding.UTF8.GetBytes(key.PadRight(8, '\0'));
            this.KeyBytes = DeriveBytes(key, salt: keyBytes, length: PasswordLength);
            this.IVBytes = DeriveBytes(key, salt: keyBytes, length: IVLength);
        }
    }
}