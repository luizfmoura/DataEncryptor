using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEncryptor.Serialization
{
    public class CryptoKey
    {
        private string Key;
        private string IV;
        
        public byte[] KeyBytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(Key);
            }
        }

        public byte[] IVBytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(IV);
            }
        }

        public CryptoKey(string key, string iv)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                throw new ArgumentException(Resources.CryptographyKeyNull);
            }
            this.Key = key.PadRight(32, '\0').Substring(0, 32);
            this.IV = iv.PadRight(16, '\0').Substring(0, 16);
        }
    }
}