using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UBALDI_EmailValidation.Models;

namespace UBALDI_EmailValidation.Controllers
{
    public class EmailValidationController : Controller
    {
        //
        // GET: /EmailValidation/
        public ActionResult Index()
        {
            EmailValidation emailValidation = new EmailValidation(null);
            return View(emailValidation);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string e)
        {
            Logger.WriteLine("Email validation page launched...");
            string email = "";
            email = Request.Form["email_ToValidate"];
            EmailValidation emailValidation = new EmailValidation(email);

            emailValidation.CheckEmailIdentifier();            
            emailValidation.CheckEmailDomains();            

            ViewData["message"] = "Your email is " + (emailValidation.IsValid ? "Valid" : "Invalid");
            ViewData["message_validity"] = emailValidation.Validity;
            Logger.WriteLine("Email " + email + " is valid: " + emailValidation.IsValid + "\n" + emailValidation.Validity);
            
            return View("Index", emailValidation);
        }
	}
}