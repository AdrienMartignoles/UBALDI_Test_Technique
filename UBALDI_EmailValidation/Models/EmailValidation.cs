using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.IO;
using System.Net;

namespace UBALDI_EmailValidation.Models
{
    public class EmailValidation
    {
        public string Email { get; set; }     
        public string Identifier { get; set; }
        public List<string> Domains { get; set; }
        public string Extension { get; set; }        
        public string Validity { get; set; }
        public bool IsValid { get; set; }

        public bool CheckEmailIdentifier()
        {
            if (Email == "" || Email == null)
            {
                IsValid = false;
                return IsValid;
            }

            IsValid = Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!IsValid)
            {
                Validity += "Syntax of the email is incorrect. It should be: abc1@domain1.domain2.extension.";
            }

            Identifier = Email.Split('@')[0];

            if (Identifier == "")
            {
                Validity += "Identifier is empty.\n";
                IsValid = false;
            }
            if (!Char.IsLetter(Identifier[0]))
            {
                Validity += "Identifier should start with a letter not with " + Identifier[0] + ".\n";
                IsValid = false;
            }
            return IsValid;
        }
        public bool CheckEmailDomains()
        {
            if (!IsValid)
                return IsValid; 

            var emailSplitted = Email.Split('@')[1].Split('.');

            if (emailSplitted == null || emailSplitted.Count() < 2)
            {
                Validity += "Email should contain at least one domain and one domain extension.\n";
                IsValid = false;
                return IsValid;
            }

            Extension = emailSplitted.Last();
            Domains = emailSplitted.Take(emailSplitted.Count() - 1).ToList();

            return IsValid;
        }
        public bool IsSpam(string countryCode)
        {
            if (!System.Enum.IsDefined(typeof(CountryWhiteList), Extension.ToUpper())
                || !System.Enum.IsDefined(typeof(CountryWhiteList), countryCode.ToUpper()))
            {
                Validity += "Extension or IP country code is not valid.\n";
                IsValid = false;
            }

            if (Identifier.Count(Char.IsLetter) <= Identifier.Count(char.IsDigit))
            {
                Validity += "Identifier should have more letters than digits.\n";
                IsValid = false;
            }

            if (Domains.Count() > 3)
            {
                Validity += "Number of Domains should be less than 3.\n";
                IsValid = false;
            }

            return IsValid;
        }
        public EmailValidation(string email = "")
        {
            this.Email = email;
        }
    }
}