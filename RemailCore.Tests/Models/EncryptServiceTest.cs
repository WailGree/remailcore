using RemailCore.Library.Models;
using NUnit.Framework;
using RemailCore.Library.Services;

namespace RemailCore.Tests.Models
{
    [TestFixture]
    public class EncryptServiceTest
    {
        [Test]
        public void Encrypt_Text_ShouldNotReturnSameText()
        {
            Account account = new Account()
            {
                Password = "Password",
                Username = "username"
            };
            string encryptedText = EncryptService.Encrypt(account.Username);
            Assert.AreNotEqual(account.Username, encryptedText);
        }

        [Test]
        public void Decrypt_Text_ShouldReturnSameText()
        {
            Account account = new Account()
            {
                Password = "Password",
                Username = "username"
            };
            string encryptedText = EncryptService.Encrypt(account.Username);
            string decryptedText = EncryptService.Decrypt(encryptedText);
            Assert.AreEqual(account.Username, decryptedText);
        }
    }
}