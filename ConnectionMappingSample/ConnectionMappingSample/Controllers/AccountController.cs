using ConnectionMappingSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ConnectionMappingSample.Controllers {

    public class AccountController : Controller {

        public ViewResult Login() {

            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult PostLogin(LoginModel loginModel) {

            if (ModelState.IsValid) {

                FormsAuthentication.SetAuthCookie(loginModel.Name, true);
                return RedirectToAction("index", "home");
            }

            return View(loginModel);
        }

        [HttpPost]
        [ActionName("SignOut")]
        public ActionResult PostSignOut() {

            FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }
    }
}