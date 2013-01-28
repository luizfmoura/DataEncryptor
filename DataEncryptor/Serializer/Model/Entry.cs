using System;

namespace DataEncryptor.Model
{
    [Serializable]
    public class Entry
    {
        public string Description { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public Entry() { }

        public Entry(string description, string user, string password)
        {
            this.Description = description;
            this.User = user;
            this.Password = password;
        }
    }
}