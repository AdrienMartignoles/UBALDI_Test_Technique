using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using UBALDI_EmailValidation.Models;

namespace UBALDI_EmailValidation.Models
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ServiceSpamDetection : System.Web.Services.WebService
    {
        [WebMethod]
        public bool SpamDetection(string email)
        {
            EmailValidation emailValidation = new EmailValidation(email);
            Logger.WriteLine("ServiceSpamDetection launched...");

            emailValidation.CheckEmailIdentifier();
            emailValidation.CheckEmailDomains();
            
            try
            {
                using (var objClient = new System.Net.WebClient())
                {
                    var strFile = objClient.DownloadString("http://freegeoip.app/json/");
                    string countrycode = strFile.Substring(strFile.IndexOf("country_code")).Split(',')[0].Split('"')[2];
                    emailValidation.IsSpam(countrycode);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Error with service: " + ex.Message);
            }

            Logger.WriteLine("Email " + email + " is valid: " + emailValidation.IsValid + "\n" + emailValidation.Validity);
            return emailValidation.IsValid;
        }
    }
}
