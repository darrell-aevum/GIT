using HomesteadViewer.ViewModels.AccountModels;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ImageProcessor;
using ImageProcessor.Imaging;
using HomesteadViewer.Models;
using HomesteadViewer.SiteUtilities;

namespace HomesteadViewer.Controllers
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            string privilegeLevels = HomesteadViewer.SiteUtilities.UserSession.GetCurrentUser().IsAdmin ? "admin;user" : "user"; // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (model.ValidateLogin())
            {
                if (model.IsNewInstall && string.IsNullOrEmpty(model.FullyQualifiedDomain))
                {
                    ViewBag.Error = "You must enter your Full Quallified Domain.";
                    return View(model);
                }

                var userData = model.IsAdminUser ? "admin" : "";

                var ticket = new FormsAuthenticationTicket(1, model.UserId.ToString(), DateTime.Now, DateTime.Now.AddDays(7), false, userData, FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                if (model.IsNewInstall)
                {
                    AppSettings.FullyQualifiedDomainName = model.FullyQualifiedDomain;
                    return RedirectToAction("Setup", "Administration", new { filter = model.UserId });
                }
                return RedirectToAction("Index", "Exemptions", new { filter = "notworked" });
            }

            ViewBag.Error = "Invalid Username or Password!";
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [Route("Account/Profile/{userName?}")]
        public ActionResult Profile(string userName = null)
        {
            var currentUserId = SiteUtilities.UserSession.GetUserId();
            var model = User_DTO.Get(userName);

            return View(model);
        }


        [HttpPost]
        public ActionResult Save(User_DTO model)
        {
            if (ModelState.IsValid)
            {

                var user = User_DTO.UpdateUser(model);
                return RedirectToAction("Profile", new { userName = user.UserName });
            }
            return View(model);
        }

        public ActionResult UserList()
        {
            return View();
        }
    }
}
