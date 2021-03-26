# RemailCore

Class Library of project "Remail". Contains MailService and Mail class for storing and managing emails. Made in .NET Standard 2.1


Services:
  - [MailService](#mailservice)
  - [EncryptService](#encryptservice)
 
Models:
  - [Account](#account)
  - [Email](#email)

## MailService
Contains authorization, email requests, and creation. Heart of this library. Only Gmail compatible. Dependencies include MailKit and MimeKit.

Example:

Check if user credentials are correct:
```C#
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

```


## EncryptService
Encrypts data for the purpose of hiding user credentials. Uses it's own passphrase for encrypting/decrypting.

## Account
Class that contains user credentials for use of mail server.

## Email
Class structure for storing emails.
