using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RemailCore.Library.Models;


namespace RemailCore.Library.Services
{
    public class MailService
    {
        private static List<Email> _emails = new List<Email>();
        private static string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Remail";

        public List<Email> GetMails(string username, string password, bool checkBackup = false)
        {
            _emails = new List<Email>();
            if (CheckInternet())
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate(username, password);
                    //The Inbox folder is always available on all IMAP servers...
                    IMailFolder inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    AddEmailsToList(client);

                    client.Disconnect(true);
                    if (checkBackup)
                    {
                        NewBackup(_emails);
                    }
                }
            }
            else
            {
                if (checkBackup)
                {
                    _emails = LoadBackup();
                }
            }

            _emails.Reverse();
            return _emails;
        }

        private bool CheckInternet()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        private static void AddEmailsToList(ImapClient client)
        {
            var uniqueIdList = client.Inbox.Search(SearchQuery.All);
            foreach (UniqueId id in uniqueIdList)
            {
                var info = client.Inbox.Fetch(new[] {id}, MessageSummaryItems.Flags);
                var seen = info[0].Flags.Value.HasFlag(MessageFlags.Seen);
                var mail = client.Inbox.GetMessage(id);

                Email email = new Email(seen, mail.From.ToString(), mail.Subject, mail.Date.DateTime, mail.TextBody,
                    (int) id.Id);
                _emails.Add(email);
            }
        }

        public void SetEmailSeen(UniqueId uId, string username, string password)
        {
            if (CheckInternet())
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate(username, password);
                    //The Inbox folder is always available on all IMAP servers...
                    IMailFolder inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    inbox.AddFlags(uId, MessageFlags.Seen, true);
                    client.Disconnect(true);
                }
            }
        }


        public void SendNewEmail(string username, string password, string text, string subject, string toMail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(username));
            message.To.Add(new MailboxAddress(toMail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = text
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("imap.gmail.com", 465, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(username, password);

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public bool IsCorrectLoginCredentials(string username, string password)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate(username, password);
                    client.Disconnect(true);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void NewBackup(List<Email> emails, string path = "EmailBackup.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            List<Email> encremails = EncryptEmails(emails);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<Email>));
                xs.Serialize(sw, encremails);
            }
        }

        private static List<Email> EncryptEmails(List<Email> unencryptedEmails)
        {
            List<Email> encryptedEmails = new List<Email>();
            foreach (Email unencryptedEmail in unencryptedEmails)
            {
                Email encryptedEmail = new Email(unencryptedEmail.Unread, unencryptedEmail.From,
                    unencryptedEmail.Subject, unencryptedEmail.Date, unencryptedEmail.Body, unencryptedEmail.UId);
                encryptedEmail.From = EncryptService.Encrypt(unencryptedEmail.From);
                encryptedEmail.Subject = EncryptService.Encrypt(unencryptedEmail.Subject);
                encryptedEmail.Body = EncryptService.Encrypt(unencryptedEmail.From);
                encryptedEmails.Add(encryptedEmail);
            }

            return encryptedEmails;
        }

        public static List<Email> LoadBackup(string path = "EmailBackup.xml")
        {
            string filePath = Path.Combine(_path, path);

            if (File.Exists(filePath))
            {
                using (StreamReader sw = new StreamReader(filePath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<Email>));
                    List<Email> encryptedEmails = (List<Email>) xs.Deserialize(sw);
                    List<Email> decryptedEmails = DecryptEmails(encryptedEmails);
                    return decryptedEmails;
                }
            }

            return null;
        }

        private static List<Email> DecryptEmails(List<Email> encryptedEmails)
        {
            List<Email> decryptedEmails = new List<Email>();
            foreach (Email encryptedEmail in encryptedEmails)
            {
                Email decryptedEmail = new Email(encryptedEmail.Unread, encryptedEmail.From, encryptedEmail.Subject,
                    encryptedEmail.Date, encryptedEmail.Body, encryptedEmail.UId);
                decryptedEmail.From = EncryptService.Decrypt(encryptedEmail.From);
                decryptedEmail.Subject = EncryptService.Decrypt(encryptedEmail.Subject);
                decryptedEmail.Body = EncryptService.Decrypt(encryptedEmail.Body);
                decryptedEmails.Add(decryptedEmail);
            }

            return decryptedEmails;
        }
    }
}