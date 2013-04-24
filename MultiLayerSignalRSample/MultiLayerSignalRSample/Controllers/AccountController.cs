using System.Web.Mvc;
using System.Web.Security;
using MultiLayerSignalRSample.Domain;
using MultiLayerSignalRSample.Models;
using MultiLayerSignalRSample.Domain.Services;

namespace MultiLayerSignalRSample.Controllers {

    public class AccountController : Controller {

        private readonly IMembershipService _membershipService;

        public AccountController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet]
        public ViewResult Login() {

            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPost(LoginModel loginModel) {

            if (ModelState.IsValid)
            {
                ValidUserContext validationContext = _membershipService.ValidateUser(loginModel.Name, loginModel.Password);
                if (validationContext.IsValid())
                {
                    FormsAuthentication.SetAuthCookie(loginModel.Name, true);
                    return RedirectToAction("index", "Home");
                }
            }

            return View(loginModel);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterPost(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                OperationResult<UserWithRoles> operationResult = _membershipService.CreateUser(registerModel.UserName, registerModel.Email, registerModel.Password, RoleConstants.CHAT_USER_ROLE_NAME);
                if(operationResult.IsSuccess)
                {
                    FormsAuthentication.SetAuthCookie(registerModel.UserName, true);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "User registration failed and we don't know why. Suck it up!");
            return View(registerModel);
        }

        [HttpPost]
        [ActionName("SignOut")]
        [ValidateAntiForgeryToken]
        public ActionResult SignOutPost() {

            FormsAuthentication.SignOut();
            return RedirectToAction("index", "Home");
        }
    }
}