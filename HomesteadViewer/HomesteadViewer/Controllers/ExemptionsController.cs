using AutoMapper;
using HomesteadViewer.Lists;
using HomesteadViewer.Models;
using HomesteadViewer.SiteUtilities;
using HomesteadViewer.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HomesteadViewer.SiteUtilities;

namespace HomesteadViewer.Controllers
{
    [Authorize]

    public class ExemptionsController : Controller
    {

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        /// <summary>
        /// Index Action
        /// </summary>
        /// <param name=></param>
        /// <returns></returns>
        public ActionResult Index(string filter = null)
        {            
            ViewBag.Filter = (filter ?? "notworked").ToLower();

            switch ((string) ViewBag.Filter)
            {
                case "unassigned":
                    ViewBag.Title = "Exemptions - Unassigned";
                    break;
                case "assigned":
                    ViewBag.Title = "Exemptions - Assigned";
                    break;
                case "approved":
                    ViewBag.Title = "Exemptions - Approved";
                    break;
                case "pending":
                    ViewBag.Title = "Exemptions - Pending";
                    break;
                case "notworked":
                    ViewBag.Title = "Exemptions - Not Worked";
                    break;
                case "all":
                    ViewBag.Title = "Exemptions - All";
                    break;
                default:
                    ViewBag.Title = "Exemptions";
                    break;
            }
            
            return View();
        }

        public ActionResult Exemption(int onlineExemptionID)
        {
            var exemption = new Exemption(MasterExemption_DTO.GetMasterExemption_For(onlineExemptionID), AdministrativeProperties_DTO.Get(onlineExemptionID));
            return View(exemption);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Exemption_List([DataSourceRequest] DataSourceRequest request, string modifier = null)
        {
            try
            {
                UserSession.LogSessionTime();
                var model = ViewModels.Exemption.Load(modifier);
                var result = model.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddOrUpdateExemption(ViewModels.Exemption data)
        {
            try
            {                
                Mapper.CreateMap<ViewModels.Exemption, AdministrativeProperties_DTO>();
                AdministrativeProperties_DTO.AddOrUpdate(Mapper.Map<ViewModels.Exemption, AdministrativeProperties_DTO>(data));
                UserSession.LogSessionTime();
                return Json(new object(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        
        public ActionResult Get_Associate_File_URLs(string id)
        {
            try
            {
                var json = Json(Helper.GetAssociatedExemptionFiles(id));
                return json;
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        //public ActionResult UpdateQuickRef(string id)
        //{
        //    try
        //    {
        //        foreach (var ap in AdministrativeProperties_DTO.GetAll())
        //        {
        //            var me = MasterExemption_DTO.GetMasterExemption_For(ap.OnlineExemptionID);
        //            if (me != null)
        //                ap.QuickrefID = me.QuickrefID;
        //            AdministrativeProperties_DTO.AddOrUpdate(ap);
        //        }
        //        return Json(new object(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Log();
        //        return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
        //    }
        //}
        public ActionResult GetAutoCompleteList()
        {
            var exemptions = MasterExemption_DTO.GetAll().GroupBy(x=>x.QuickrefID).Select(x=>x.First());
            return Json(exemptions, JsonRequestBehavior.AllowGet);
        }
    }
}