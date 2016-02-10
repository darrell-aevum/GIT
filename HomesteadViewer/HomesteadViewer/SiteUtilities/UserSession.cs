using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using HomesteadViewer.Models;
using HomesteadViewer.DAL;

namespace HomesteadViewer.SiteUtilities
{
    public class UserSession
    {
        public static User_DTO GetCurrentUser()
        {
            var currentUserId = GetUserId();            
            return User_DTO.Get(currentUserId) ?? new User_DTO();            
        }

        public static int GetUserId()
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            if (!HttpContext.Current.User.Identity.IsAuthenticated ||
                HttpContext.Current.Request.Cookies[cookieName] == null
            )
            {
                return 0;
            }

            var authCookie = HttpContext.Current.Request.Cookies[cookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            int retVal;
            int.TryParse(authTicket.Name, out retVal);
            return retVal;
        }
        
        public static bool IsUserAdmin()
        {
            var authTicket = GetAuthTicket();
            if (authTicket == null) { return false; }

            return authTicket.UserData.Contains("admin") || authTicket.UserData.Contains("system");
        }
        public static UserTimeLog_DTO LogSessionTime()
        {
            try
            {
                var user = GetCurrentUser();
                var lastLog = UserTimeLog_DTO.GetLast(user.Id);
                //Create New
                if (lastLog == null)
                {
                    return UserTimeLog_DTO.Add(user.Id);
                }

                var newTimestamp = DateTime.Now;

                TimeSpan span = newTimestamp - lastLog.FirstLogDate;
                //if timespan > 15 minutes Create New
                if (span.TotalMinutes > 15)
                {
                    return UserTimeLog_DTO.Add(user.Id);
                }

                //Update
                lastLog.LastLogDate = newTimestamp;
                lastLog.Duration += (int)span.TotalMilliseconds;
                return UserTimeLog_DTO.Update(lastLog);
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }

        private static FormsAuthenticationTicket GetAuthTicket()
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            if (!HttpContext.Current.User.Identity.IsAuthenticated ||
                HttpContext.Current.Request.Cookies[cookieName] == null
            )
            {
                return null;
            }

            var authCookie = HttpContext.Current.Request.Cookies[cookieName];
            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            return authTicket;
        }
    }
}