using HomesteadViewer.Lists;
using HomesteadViewer.Models;
using HomesteadViewer.SiteUtilities;
using HomesteadViewer.ViewModels.Administration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
namespace HomesteadViewer.Controllers
{
    [AuthorizeUser(AccessLevel = "admin")]
    public class AdministrationController : Controller
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
        public ActionResult Index()
        {
            //var exemptions = DataHelper.GetExemptions();
            //int count = exemptions.Count();
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Setup()
        {
            return View(new ApplicationSettingsViewModel());
        }
        public ActionResult ApplicationSettings()
        {
            return View(new ApplicationSettingsViewModel());
        }
        public ActionResult ErrorLogs()
        { 
            return View();
        }
        public ActionResult OrphanedData()
        { 
            return View();
        }
        public ActionResult SaveApplicationSettings(ApplicationSettingsViewModel settings)
        {
            AppSettings.AnyoneCanRegister = settings.AnyoneCanRegister;
            AppSettings.ApprovalForAccess = settings.ApprovalForAccess;
            AppSettings.Catalog = settings.Catalog;
            AppSettings.DataSource = settings.DataSource;
            AppSettings.FullyQualifiedDomainName = settings.FullyQualifiedDomainName;
            AppSettings.FileRepositoryPath = settings.FileRepositoryPath;
            AppSettings.SearchTermTableColumn = settings.SearchTermTableColumn;
            AppSettings.SiteTitle = settings.SiteTitle;
            UserSession.LogSessionTime(); 
            return RedirectToAction("Index", "Administration");
        }

        public ActionResult Users()
        {
            return View();
        }
 
        public ActionResult GridSettings()
        {
            return View(new GridSettingsViewModel());
        } 
        public ActionResult NewUser()
        {
            return View();
        } 

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult User_Update([DataSourceRequest] DataSourceRequest request, Models.User_DTO user)
        {
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    User_DTO.UpdateUser(user);
                }
                UserSession.LogSessionTime();
                return Json(new[] { user }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult User_Create([DataSourceRequest] DataSourceRequest request, Models.User_DTO user)
        {
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    User_DTO.AddUser(user);
                }
                UserSession.LogSessionTime();
                return Json(new[] { user }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult User_Delete([DataSourceRequest] DataSourceRequest request, Models.User_DTO user)
        {
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    User_DTO.DeleteUser(user.Id);
                }
                UserSession.LogSessionTime();
                return Json(new[] { user }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }


        public ActionResult User_List([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = User_DTO.GetAll();
                var result = model.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        public ActionResult GridColumnList_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = GridColumn_DTO.GetAll().OrderBy(gc => gc.ColumnNumber);
                var result = model.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        public ActionResult ErrorLogs_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = ErrorLog_DTO.GetAll().OrderByDescending(e => e.TimeStamp);
                var result = model.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        public ActionResult OrphanedData_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = AdministrativeProperties_DTO.GetOrphanedData().OrderByDescending(ap => ap.ID);
                var result = model.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        public ActionResult Update_GridColumns([DataSourceRequest] DataSourceRequest request, GridColumn_DTO model)
        {
            try
            {
                model = GridColumn_DTO.UpdateGridColumn(model);
                UserSession.LogSessionTime();
                return Json(new
                {
                    result = AjaxRequestResults.success.ToString(),
                    message = "Successfully Updated The Grid Column " + model.DisplayName
                });
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }

        public ActionResult UserList_Read([DataSourceRequest] DataSourceRequest request = null)
        {
            try
            {
                var model = User_DTO.GetAll();
                DataSourceResult result = model.ToDataSourceResult(request); 
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }

        public ActionResult ClearLogs([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ErrorLog_DTO.RemoveAll();
                UserSession.LogSessionTime();
                return Json(new
                {
                    result = AjaxRequestResults.success.ToString(),
                    message = "Successfully Removed All Error Logs"
                });
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
        [AuthorizeUser(AccessLevel = "user")]
        public ActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                Mapper.CreateMap<User_DTO, User_DTO>().ForMember(dest => dest.AssignedExemptions, opt => opt.Ignore());
                var users = User_DTO.GetAll();

                ViewBag.Users = users.Select(u => Mapper.Map<User_DTO, User_DTO>(u)).OrderBy(u => u.FullName).ToList();
                var result = users.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.Log();
                return Json(new { result = AjaxRequestResults.error.ToString(), message = ex.Message });
            }
        }
    }
}