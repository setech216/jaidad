using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DbLayer.db;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ModelLayer;
using PropertyProject.Models;


namespace PropertyProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        UserDb userDb;

        public AccountController()
        {
            userDb = new UserDb();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Account/Settings
        [AllowAnonymous]
        public ActionResult Settings()
        {
            if ((Request.Cookies["user"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //Session["userObj"] = userDb.getUserModel(model.Email);
                    UserModel userModel= userDb.getUserModel(model.Email);
                    HttpCookie cookie = new HttpCookie("user");
                    cookie.Values.Add("id", userModel.Id);
                    cookie.Values.Add("email", userModel.Email);
                    cookie.Values.Add("name", userModel.Name);
                    cookie.Values.Add("phone", userModel.PhoneNumber);
                    cookie.Values.Add("roleId", userModel.roleId.ToString());
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,name=model.Name, PhoneNumber=model.Phone ,roleId= (int)Role.user };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //Session["userObj"]= userDb.getUserModel(user.Email);
                    UserModel userModel = userDb.getUserModel(model.Email);
                    HttpCookie cookie = new HttpCookie("user");
                    cookie.Values.Add("id", userModel.Id);
                    cookie.Values.Add("email", userModel.Email);
                    cookie.Values.Add("name", userModel.Name);
                    cookie.Values.Add("phone", userModel.PhoneNumber);
                    cookie.Values.Add("roleId", userModel.roleId.ToString());
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            if ((Request.Cookies["user"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction( "Login", "Account");
            }
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if ((Request.Cookies["user"] != null))
            {
                model.Email = @Request.Cookies["user"]["email"];
                var user = await UserManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    ModelState.AddModelError(string.Empty, "something went wrong!!");
                }
                if (UserManager.CheckPassword(user, model.OldPassword))//checking that user entered correct old password
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    model.Code = code;
                    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "password changed successfully!!!");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "old password is incorrect");
                }

            }
            else {
                ModelState.AddModelError(string.Empty, "something went wrong!!");
            }
            
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpGet]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            //Session.Remove("userObj");
            if (Request.Cookies["user"] != null)
            {
                HttpCookie nameCookie = Request.Cookies["user"];  
                nameCookie.Expires = DateTime.Now.AddDays(-1);  
                Response.Cookies.Add(nameCookie);
            }
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        // GET: /Account/ResetPhone
        [AllowAnonymous]
        public ActionResult ResetPhone()
        {
            if ((Request.Cookies["user"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/ResetPhone
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPhone(string phone)
        {

            if ((Request.Cookies["user"] != null))
            {
                string id = Request.Cookies["user"]["id"];
                userDb.updatePhone(id, phone);
                HttpCookie cookie = Request.Cookies["user"];
                cookie.Values["phone"] = phone;
                Response.AppendCookie(cookie);
                ViewBag.resetPhone = "phone No changed successfully!";
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // GET: /Account/ResetEmail
        [AllowAnonymous]
        public ActionResult ResetEmail()
        {
            if ((Request.Cookies["user"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/ResetPhone
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetEmail(string email)
        {

            if ((Request.Cookies["user"] != null))
            {
                string id = Request.Cookies["user"]["id"];
                userDb.updateEmail(id, email);
                HttpCookie cookie = Request.Cookies["user"];
                cookie.Values["email"] = email;
                Response.AppendCookie(cookie);
                ViewBag.resetPhone = "username and email address has changed successfully!";
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // GET: /Account/ResetName
        [AllowAnonymous]
        public ActionResult ResetName()
        {
            if ((Request.Cookies["user"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // POST: /Account/ResetName
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetName(string name)
        {

            if ((Request.Cookies["user"] != null))
            {
                string id = Request.Cookies["user"]["id"];
                userDb.updateName(id, name);
                HttpCookie cookie = Request.Cookies["user"];
                cookie.Values["name"] = name;
                Response.AppendCookie(cookie);
                ViewBag.resetPhone = "Name has changed successfully!";
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        // GET: /Account/UserProfile
        [AllowAnonymous]
        public ActionResult UserProfile()
        {
            if ((Request.Cookies["user"] != null))
            {
                string id = Request.Cookies["user"]["id"];
                UserModel model;
                if (id != null)
                {
                    model = userDb.getUserById(id);
                }
                else
                {
                    model = new UserModel();
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AllowAnonymous]
        public ActionResult Manage()
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.superAdmin))
                {
                    return View();
                }
                else {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [AllowAnonymous]
        public ActionResult FindAccount()
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.superAdmin))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetAccountAndUpgrade(string email)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.superAdmin))
                {
                    UserModel model= userDb.getUserModel(email);
                    
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetAccountAndUpgrade(int role)
        {
            if ((Request.Cookies["user"] != null))
            {
                int roleId;
                Int32.TryParse(Request.Cookies["user"]["roleId"], out roleId);
                if ((roleId == (int)Role.superAdmin))
                {
                    string userId = Request.Form["userId"];
                    string email = Request.Form["email"];
                    bool flag = userDb.makeAgent(userId, role);
                    if(flag==true)
                    {
                        ViewBag.msg = "account updated";
                    }
                    else
                    {
                        ViewBag.msg = "oops! something went wrong";
                    }
                    return RedirectToAction("GetAccountAndUpgrade", "Account", new { @email = email });
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}