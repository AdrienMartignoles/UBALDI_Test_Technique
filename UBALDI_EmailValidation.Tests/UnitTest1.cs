using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UBALDI_EmailValidation.Models;

namespace UBALDI_EmailValidation.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_Empty()
        {
            EmailValidation e = new EmailValidation("");
            e.CheckEmailIdentifier();
            Assert.AreEqual(false, e.IsValid);
        }

        [TestMethod]
        public void Test_ShouldStartWithALetter()
        {
            EmailValidation e = new EmailValidation("1test@test.com");
            e.CheckEmailIdentifier();
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("test@test.com");
            e.CheckEmailIdentifier();
            Assert.AreEqual(true, e.IsValid);
        }

        [TestMethod]
        public void Test_CorrectEmails()
        {
            EmailValidation e = new EmailValidation("niceandsimple@example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(true, e.IsValid);

            e = new EmailValidation("a.little.unusual@example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(true, e.IsValid);

            e = new EmailValidation("a.little.more.unusual@dept.example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(true, e.IsValid);
        }

        [TestMethod]
        public void Test_IncorrectEmails()
        {
            EmailValidation e = new EmailValidation("abc");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("Abc.example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("Abc.@example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("Abc..123@example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("A@b@c@example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(false, e.IsValid);
            
            e = new EmailValidation("a\"b(c)d,e:f;g<h>i[j\\k]l@example.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            Assert.AreEqual(false, e.IsValid);
        }

        [TestMethod]
        public void Test_Extensions()
        {
            EmailValidation e = new EmailValidation("abc@test.com");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("US");
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("abc@test.fr");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("fr");
            Assert.AreEqual(true, e.IsValid);

            e = new EmailValidation("abc@test.de");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("de");
            Assert.AreEqual(true, e.IsValid);
        }

        [TestMethod]
        public void Test_WebService()
        {
            using (var objClient = new System.Net.WebClient())
            {
                var strFile = objClient.DownloadString("http://freegeoip.app/json/");
                string countrycode = strFile.Substring(strFile.IndexOf("country_code")).Split(',')[0].Split('"')[2];
                EmailValidation e = new EmailValidation("abc@test.fr");
                e.CheckEmailIdentifier();
                e.CheckEmailDomains();
                e.IsSpam(countrycode);
                Assert.AreEqual(true, e.IsValid);
            }        
        }

        [TestMethod]
        public void Test_WebServiceMock()
        {
            using (var objClient = new System.Net.WebClient())
            {
                var strFile = objClient.DownloadString("http://freegeoip.app/json/42.42.42.42");
                string countrycode = strFile.Substring(strFile.IndexOf("country_code")).Split(',')[0].Split('"')[2];
                EmailValidation e = new EmailValidation("abc@test.fr");
                e.CheckEmailIdentifier();
                e.CheckEmailDomains();
                e.IsSpam(countrycode);
                Assert.AreEqual(false, e.IsValid);
            }
        }

        [TestMethod]
        public void Test_Domains()
        {
            EmailValidation e = new EmailValidation("a1b2c@test.test.test.1.fr");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("FR");
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("a1b2c@test.test.test.fr");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("FR");
            Assert.AreEqual(true, e.IsValid);
        }

        [TestMethod]
        public void Test_LettersDigits()
        {
            EmailValidation e = new EmailValidation("a1b2c3@test.fr");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("FR");
            Assert.AreEqual(false, e.IsValid);

            e = new EmailValidation("a1b2c@test.fr");
            e.CheckEmailIdentifier();
            e.CheckEmailDomains();
            e.IsSpam("FR");
            Assert.AreEqual(true, e.IsValid);
        }
    }
}
