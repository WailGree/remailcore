using NUnit.Framework;
using RemailCore.Library.Models;

namespace RemailCore.Tests.Models
{
    [TestFixture]
    public class AccountTests
    {
        private Account account;

        [SetUp]
        public void Setup()
        {
            account = new Account();
            account.Setup("username", "password", false);
        }

        [Test]
        public void Setup_Username_ShouldReturnUsername()
        {
            Assert.AreEqual("username", account.Username);
        }

        [Test]
        public void Setup_Password_ShouldReturnPassword()
        {
            Assert.AreEqual("password", account.Password);
        }

        [Test]
        public void SaveCredentials_Account_ShouldReturnAccount()
        {
            Account.SaveCredentials(account, "CredentialsTest.xml");
            Account accountUnderTest = Account.LoadCredentials("CredentialsTest.xml");
            Assert.AreEqual(account, accountUnderTest);
        }


        [Test]
        public void LoadCredentials_Account_ShouldReturnAccount()
        {
            Account.SaveCredentials(account, "CredentialsTest.xml");
            Account accountUnderTest = Account.LoadCredentials("CredentialsTest.xml");
            Assert.AreEqual(account, accountUnderTest);
        }
    }
}