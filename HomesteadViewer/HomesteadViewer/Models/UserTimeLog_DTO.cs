using HomesteadViewer.DAL;
using HomesteadViewer.Lists;
using HomesteadViewer.SiteUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using HomesteadViewer.Helpers;
using AutoMapper;

namespace HomesteadViewer.Models
{ 

    [Table("UserTimeLogs")]
    public class UserTimeLog_DTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime FirstLogDate { get; set; }
        public DateTime LastLogDate { get; set; }
        public int Duration { get; set; }

        public static UserTimeLog_DTO Get(int id)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.UserLogs.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static UserTimeLog_DTO Get(Func<UserTimeLog_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.UserLogs.Find(func);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static UserTimeLog_DTO GetLast(int userId)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.UserLogs.OrderByDescending(ul => ul.Id).FirstOrDefault(ul => ul.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static UserTimeLog_DTO GetLast(Func<UserTimeLog_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.UserLogs.OrderByDescending(ul => ul.Id).FirstOrDefault(func);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<UserTimeLog_DTO> GetAll()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                { 
                    return context.UserLogs.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<UserTimeLog_DTO> GetAll(Func<UserTimeLog_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.UserLogs.Where(func).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int Count()
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.UserLogs.Count();
            }
        }
        public static int Count(Func<UserTimeLog_DTO, bool> func)
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.UserLogs.Count(func);
            }
        }
        public static UserTimeLog_DTO Update(UserTimeLog_DTO userLog)
        {
            Mapper.CreateMap<Models.UserTimeLog_DTO, Models.UserTimeLog_DTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                try
                {
                    var c = Get(userLog.Id);
                    if (c == null)
                        throw new Exception("User Could Not Be Found");
                    Mapper.Map<Models.UserTimeLog_DTO, Models.UserTimeLog_DTO>(userLog, c);

                    TimeSpan span = c.LastLogDate - c.FirstLogDate;
                    c.Duration = (int)span.TotalMilliseconds;

                    context.UserLogs.Attach(c);
                    context.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return Get(userLog.Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static UserTimeLog_DTO Add(int userId)
        {
            if (!User_DTO.Exists(userId))
                return null;
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var userLog = new UserTimeLog_DTO()
                    {
                         FirstLogDate = DateTime.Now,
                         LastLogDate = DateTime.Now,
                         Duration = 0,
                          UserId = userId
                    };

                    context.UserLogs.Add(userLog);
                    context.SaveChanges();
                    
                    return GetLast(userId);
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

                    context.Database.ExecuteSqlCommand("TRUNCATE TABLE [UserLogs]");
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