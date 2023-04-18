using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CarParts.Models.Account;
using System.Text;
using CarParts.Attributes;
using CarParts.Enums;

namespace CarParts.Controllers
{
    public partial class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.RoleList = //Konwersja enum Role na listę
                Enum.GetValues(typeof(Role)).Cast<Role>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if(UserNameExsist(model.UserName))
            {
                ModelState.AddModelError("UserName", "Konto o podanym loginie już istnieje");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                using (CarPartsEntities db = new CarPartsEntities())
                {
                    db.Accounts.Add(
                        new Account() 
                        { 
                            Email = model.Email, 
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Password = CreateHash(model.Password),
                            UserName = model.UserName,
                            AccountRole = model.AccountRole
                        });
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        private string CreateHash(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            using (CarPartsEntities db = new CarPartsEntities())
            {
                var account = db.Accounts.Where(a => !a.Deleted && a.UserName == model.UserName).FirstOrDefault();
                if (account != null)
                {
                    if (string.Compare(CreateHash(model.Password), account.Password) == 0)
                    {
                        int timeout = 60;// login.RememberMe ? 525600 : 20;
                        //var ticket = new FormsAuthenticationTicket(model.UserName, false, timeout);
                        var ticket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, DateTime.Now.AddMinutes(timeout), false, account.AccountRole.ToString(), FormsAuthentication.FormsCookiePath);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Błędne hasło");                        
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "Nie znaleziono takiego użytkownika");
                    return View(model);
                }
            }

            TempData["SuccesMessage"] = "Witamy ponownie!";

            return RedirectToAction("Index", "Home");
        }

        [AuthRole]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ActionName("ChangePassword")]
        public ActionResult ChangePasswordPOST(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (CarPartsEntities db = new CarPartsEntities())
                    {
                        var account = db.Accounts.Where(a => a.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                        if (account != null)
                        {
                            if (string.Compare(CreateHash(model.OldPassword), account.Password) != 0)
                            {
                                ModelState.AddModelError("OldPassword", "Stare hasło nie pasuje");
                                return View(model);
                            }

                            if (string.Compare(model.NewPassword, model.ConfirmPassword) == 0)
                            {
                                account.Password = CreateHash(model.NewPassword);
                                db.SaveChanges();

                                TempData["SuccesMessage"] = "Hasło zostało zmienione!";
                            }
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [NonAction]
        private bool UserNameExsist(string userName)
        {
            using (CarPartsEntities  db = new CarPartsEntities())
            {
                return db.Accounts.Where(a => a.UserName == userName).FirstOrDefault() != null;
            }
        }

        /*[NonAction]
        public void SendMail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("sameeranihathe@gmail.com", "sameera sampath");
            var toEmail = new MailAddress(emailID);
            var fromemailPassword = "kanchana143";
            string subject = "Your account is successfully created";

            string body = "<br/> <br/> Your account is successfully created. Please click the below link to verify the account" +
                "<br/> <br/> <a href='" + link + "'>" + link + "</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromemailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }*/


    }
}
