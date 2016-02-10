using HomesteadViewer.DAL;
using HomesteadViewer.Lists;
using HomesteadViewer.SiteUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using HomesteadViewer.Helpers;

namespace HomesteadViewer.Models
{ 

    [Table("ErrorLogs")]
    public class ErrorLog_DTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public int? LineNumber { get; set; }
        public string MethodName { get; set; }
        public string Classname { get; set; }
        public string AdditionalIfno { get; set; }
        public string InnerException { get; set; }

        public static ErrorLog_DTO Get(int id)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.ErrorLogs.FirstOrDefault(l => l.Id == id);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<ErrorLog_DTO> GetAll()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                { 
                    return context.ErrorLogs.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static ErrorLog_DTO Get(Func<ErrorLog_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.ErrorLogs.FirstOrDefault(func);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<ErrorLog_DTO> GetAll(Func<ErrorLog_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.ErrorLogs.Where(func).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static int Count()
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.ErrorLogs.Count();
            }
        }   
        public static ErrorLog_DTO Add(Exception exception, string additionalInfo = null)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    ErrorLog_DTO ErrorLog = new ErrorLog_DTO()
                    {
                       UserId = exception.UserId(),
                        Message = exception.Message,
                        TimeStamp = exception.TimeStamp(),
                        Classname = exception.ClassName(),
                        LineNumber = exception.LineNumber(),
                        MethodName = exception.MethodName(),
                        InnerException = (exception.InnerException != null) ? exception.InnerException.Message : "",
                        AdditionalIfno = additionalInfo
                    };

                    context.ErrorLogs.Add(ErrorLog);
                    context.SaveChanges();
                    return context.ErrorLogs.FirstOrDefault(e=>e.UserId == ErrorLog.UserId && e.TimeStamp == ErrorLog.TimeStamp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ErrorLog_DTO Add(Exception exception, string className, string methodName, string additionalInfo = null)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    ErrorLog_DTO ErrorLog = new ErrorLog_DTO()
                    {
                        UserId = exception.UserId(),
                        Message = exception.Message,
                        TimeStamp = exception.TimeStamp(),
                        Classname = exception.ClassName() ?? className,
                        LineNumber = exception.LineNumber(),
                        MethodName = exception.MethodName() ?? methodName,
                        InnerException = (exception.InnerException != null) ? exception.InnerException.Message : "",
                        AdditionalIfno = additionalInfo
                    };

                    context.ErrorLogs.Add(ErrorLog);
                    context.SaveChanges();
                    return context.ErrorLogs.FirstOrDefault(e => e.UserId == ErrorLog.UserId && e.TimeStamp == ErrorLog.TimeStamp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void RemoveAll()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {

                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE [ErrorLogs]");
                    context.SaveChanges();                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}