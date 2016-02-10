using HomesteadViewer.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using HomesteadViewer.Models;

namespace HomesteadViewer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Index Action
        /// </summary>
        /// <param name=></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Exemptions", new { filter = SiteUtilities.UserSession.GetCurrentUser().Id });
        }

        /// <summary>
        /// Create Action
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create Method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(User_DTO user)
        {
            if (!ModelState.IsValid)
                return Json(new { result = "error", message = "The form is invalid" });

            var success = User_DTO.AddUser(user);

            if (!success)
                return Json(new { result = "error", message = "There was an error creating the user." });

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Edit Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var user = User_DTO.GetUser(id);
            return View(user);
        }

        /// <summary>
        /// Edit Method
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveUser(User_DTO user)
        {
            if (!ModelState.IsValid)
                return View();

            var u = User_DTO.UpdateUser(user);

            if (u == null)
                return View();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete Action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            var success = User_DTO.DeleteUser(id);

            if (!success)
                throw new Exception("User Not Deleted");

            return Json(new { result = "success", message = "User Deleted." }, JsonRequestBehavior.AllowGet);
        }
    }
}