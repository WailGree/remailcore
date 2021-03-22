using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using RemailCore.Services;

namespace RemailCore.Models
{
    public class Account : ISerializable
    {
        
        public int Id { get; set; }
        [Required] [MaxLength(100)] public string Username { get; set; }
        [Required] [MaxLength(100)] public string Password { get; set; }
        public List<Email> Emails { get; set; }
        public bool RememberUserCredentials { get; set; }
        private static string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Remail";

        public Account()
        {
        }

        public void Setup(string username, string password, bool rememberUserCredentials)
        {
            Username = username;
            Password = password;
            RememberUserCredentials = rememberUserCredentials;
        }

        public static void SaveCredentials(Account account, string path = "Credentials.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Account));
                Account encryptedAccount = EncryptAccount(account);
                xs.Serialize(sw, encryptedAccount);
            }
        }

        public static void BackupCredentials(string path = "BackupCredentials.xml")
        {
            Account account = LoadCredentials();
            SaveCredentials(account, path);
        }

        private static Account EncryptAccount(Account account)
        {
            account.Username = EncryptService.Encrypt(account.Username);
            account.Password = EncryptService.Encrypt(account.Password);
            return account;
        }

        public static Account LoadCredentials(string path = "Credentials.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                using (StreamReader sw = new StreamReader(filePath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Account));
                    Account account = (Account) xs.Deserialize(sw);
                    return DecryptAccount(account);
                }
            }

            return LoadBackupAccount();
        }

        public static Account LoadBackupAccount(string path = "BackupCredentials.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                using (StreamReader sw = new StreamReader(filePath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(Account));
                    Account account = (Account) xs.Deserialize(sw);
                    return DecryptAccount(account);
                }
            }

            return null;
        }

        private static Account DecryptAccount(Account account)
        {
            account.Username = EncryptService.Decrypt(account.Username);
            account.Password = EncryptService.Decrypt(account.Password);
            return account;
        }

        public static void DeleteCredentials(string path = "Credentials.xml")
        {
            string filePath = Path.Combine(_path, path);
            File.Delete(filePath);
        }

        public static bool SavedCredentialsFound(string path = "Credentials.xml")
        {
            string filePath = Path.Combine(_path, path);
            return File.Exists(filePath);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);
            info.AddValue("Password", Password);
            info.AddValue("RememberUserCredentials", RememberUserCredentials);
        }

        public Account(SerializationInfo info, StreamingContext context)
        {
            Username = (string) info.GetValue("Name", typeof(string));
            Password = (string) info.GetValue("Password", typeof(string));
            RememberUserCredentials = (bool) info.GetValue("RememberUserCredentials", typeof(bool));
        }
    }
}