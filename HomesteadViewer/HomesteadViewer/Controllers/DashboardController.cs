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
using HomesteadViewer.ViewModels;

namespace HomesteadViewer.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ActionResult Index()
        {
            var model = new DashboardViewModel();
            return View(model);
        }
 
    }
}
