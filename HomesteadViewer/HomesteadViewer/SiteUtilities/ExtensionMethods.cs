using System;
using HomesteadViewer.Models;
using HomesteadViewer.SiteUtilities;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;

namespace HomesteadViewer.SiteUtilities
{
    public static class ExtensionMethods
    { 
        public static int LineNumber(this Exception ex)
        {
            int linenum = 0;
            try
            {
                linenum = Convert.ToInt32(ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(":line") + 5));
            }
            catch
            {
                //Stack trace is not available!
            }
            return linenum;
        }
        public static string ClassName(this Exception ex)
        {
            try
            {
                var trace = new System.Diagnostics.StackTrace(ex, true); 
                return trace.GetFrame(0).GetMethod().ReflectedType.FullName; 
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string MethodName(this Exception ex)
        {
            try
            {
                var site = ex.TargetSite;
                string methodName = site == null ? null : site.Name;
                return methodName;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static int UserId(this Exception ex)
        {
            try
            {
                var user = UserSession.GetCurrentUser();
                int userId = 0;
                if (user != null)
                    userId = user.Id;

                return userId;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public static DateTime TimeStamp(this Exception ex)
        {
            try
            {
                return DateTime.Now;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static ErrorLog_DTO Log(this Exception ex, string additionalInfo = null)
        {
            try
            {
                return ErrorLog_DTO.Add(ex, additionalInfo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static ErrorLog_DTO Log(this Exception ex, string className, string methodName, string additionalInfo = null)
        {
            try
            {
                return ErrorLog_DTO.Add(ex, additionalInfo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //public static ErrorLog_DTO.ErrorLog Log(this Exception ex, ErrorLog_DTO.LogType logType, ErrorLog_DTO.EventType eventTpye, string customMessage = null)
        //{
        //    try
        //    {
        //        var log = BaseLog(ex);
        //        log.LogType = logType;
        //        log.Message = (string.IsNullOrEmpty(customMessage)) ? ex.Message : customMessage + " | Exception: " + ex.Message;
        //        log.EventType = eventTpye;
        //        LogsUtility.ErrorLogsUtility.AddLog(log);
        //        return log;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public static ErrorLog_DTO.ErrorLog Log(this Exception ex, ErrorLog_DTO.LogType logType, string customMessage = null)
        //{
        //    try
        //    {
        //        var log = BaseLog(ex);
        //        log.LogType = logType;
        //        log.Message = (string.IsNullOrEmpty(customMessage)) ? ex.Message : customMessage + " | Exception: " + ex.Message;

        //        LogsUtility.ErrorLogsUtility.AddLog(log);
        //        return log;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public static ErrorLog_DTO.ErrorLog Log(this Exception ex, ErrorLog_DTO.LogType logType, int userId, string customMessage = null)
        //{
        //    try
        //    {
        //        var log = BaseLog(ex);
        //        log.LogType = logType;
        //        log.Message = (string.IsNullOrEmpty(customMessage)) ? ex.Message : customMessage + " | Exception: " + ex.Message;
        //        log.UserId = userId;
        //        LogsUtility.ErrorLogsUtility.AddLog(log);
        //        return log;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

    }
  
}