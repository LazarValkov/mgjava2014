using MgJava2014.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MgJava2014.Models.DbModel;

namespace MgJava2014.Controllers
{
    public class AccountController : Controller
    {

        private MainDBEntities _dbConnection;
        private MainDBEntities dbConnection { get { return _dbConnection ?? (_dbConnection = new MainDBEntities()); } }
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel acc)
        {
            if(ModelState.IsValid)
            {
                var areCredentialsCorrect = dbConnection.Users.Any(user => user.username == acc.Username && user.password == acc.Password);
                if (areCredentialsCorrect)
                {
                    FormsAuthentication.SetAuthCookie(acc.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorMsg = "Грешен потребител/парола.";

            }
            return View();
            //FormsAuthentication.SetAuthCookie("lgpz", true);
            
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var doesUserAlreadyExist = dbConnection.Users.Any(user => user.username == model.Username);
                if (!doesUserAlreadyExist)
                {
                    var newUser = new User { username = model.Username, password = model.Password, email = model.Email, Score = new Score() };
                    dbConnection.Users.Add(newUser);
                    dbConnection.SaveChanges();
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorMsg = "Това име е заето.";
            }
            return View(model);
        }

        
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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
    }
}
