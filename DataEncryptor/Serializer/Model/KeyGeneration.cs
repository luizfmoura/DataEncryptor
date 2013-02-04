using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataEncryptor.Model
{
    public class KeyGeneration
    {
        public const string DefaultChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public int PasswordLength { get; set; }

        public string AllowableChars { get; set; }

        public KeyGeneration(int passwordLength, string allowableChars = null)
        {
            PasswordLength = passwordLength;
            AllowableChars = allowableChars ?? DefaultChars;
        }

        public string GeneratePassword()
        {
            StringBuilder passwordBuilder = new StringBuilder((int)PasswordLength);
            var r = new Random();
            for (int i = 0; i < PasswordLength; i++)
            {
                int nextInt = r.Next(AllowableChars.Length);
                char c = AllowableChars[nextInt];
                passwordBuilder.Append(c);
            }
            return passwordBuilder.ToString();
        }
    }
}
