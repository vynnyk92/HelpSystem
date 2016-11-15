using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HelpDesk.WebUI.Concrete;
using HelpDesk.WebUI.Entities;
using HelpDesk.WebUI.Providers;
using HelpDesk.WebUI.Models;

namespace HelpDesk.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else { ModelState.AddModelError("","Wrong login or password"); }
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }

        private bool ValidateUser(string login, string password)
        {

            bool isValid = false;
            using (HelpdeskContext db = new HelpdeskContext())
            {
                try
                {
                    User user = db.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();
                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch { isValid = false; }
            }
            return isValid;
        }
    }
}