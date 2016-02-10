using HomesteadViewer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using HomesteadViewer.Models;
using System.Linq;
using System.Collections.Generic;
namespace HomesteadViewer.DAL
{
    public class DataHelper
    {
        /// <summary>
        /// Gets a user by int id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static User GetUser(int id, bool? ignoreRelatedObjects = null)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    if (ignoreRelatedObjects != null && (bool)ignoreRelatedObjects)
                    {
                        Mapper.CreateMap<User, User>().ForMember(dest => dest.AssignedExemptions, opt => opt.Ignore());
                        var user = context.Users.Include(u => u.AssignedExemptions).Where(u => u.Id == id).FirstOrDefault();
                        return Mapper.Map<User, User>(user);
                    }
                    return context.Users.Include(u => u.AssignedExemptions).Where(u => u.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static User GetUser(string userName)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.Include(u => u.AssignedExemptions).FirstOrDefault(c => c.UserName == userName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a user by int id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetUserCount()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.Count();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List<User></returns>
        public static List<User> GetAllUsers(bool lazyLoad = true)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    context.Configuration.LazyLoadingEnabled = lazyLoad;
                    return context.Users.Include(u => u.AssignedExemptions).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates a user's information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public static bool UpdateUser(int id, string firstName, string lastName, string phone, string email)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var user = GetUser(id);
                    if (user == null)
                        throw new Exception("User Could Not Be Found");

                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.Phone = phone;
                    user.EmailAddress = email;

                    context.Users.Attach(user);
                    context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Updates a user's information
        /// </summary>
        /// <param name="user"></param>
        /// <returns>bool</returns>
        public static User UpdateUser(User user)
        {
            Mapper.CreateMap<Models.User, Models.User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                try
                {
                    var c = GetUser(user.Id);
                    if (c == null)
                        throw new Exception("User Could Not Be Found");
                    Mapper.Map<Models.User, Models.User>(user, c);

                    context.Users.Attach(c);
                    context.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return GetUser(user.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        public static bool AddUser(string userName, string phone, string email)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    User user = new User()
                    {
                        UserName = userName,
                        Phone = phone,
                        EmailAddress = email
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>bool</returns>
        public static bool AddUser(User user)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns> 
        public static bool DeleteUser(int id)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var user = GetUser(id);

                    if (user == null)
                        return false;

                    //                    foreach (var ae in GetAssignedExemptionsByUserId(user.Id))
                    //                      UnassignExemption(ae.QuedExemptionId);

                    context.Users.Attach(user);
                    context.Users.Remove(user);
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<QuedExemption> GetQuedExemptions()
        {
            try
            {
                using (QuedExemptionsContext context = new QuedExemptionsContext())
                {
                    return context.QuedExemptions.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Exemption> GetExemptions(bool lazyLoad = true)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    context.Configuration.LazyLoadingEnabled = lazyLoad;
                    return context.Exemptions.Include(e=>e.AssignedUsers).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<Exemption> GetExemptions(User user)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Exemptions.Where(e => e.AssignedUsers.Select(u=>u.Id).Contains(user.Id)).ToList();                    
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<Exemption> GetAssignedExemptions(bool lazyLoad = true)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    context.Configuration.LazyLoadingEnabled = lazyLoad;
                    return context.Exemptions.Include(e=>e.AssignedUsers).Where(e => e.AssignedUsers.Count > 0).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Exemption> GetAssignedExemptions(int userId, bool lazyLoad = true)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {

                    context.Configuration.LazyLoadingEnabled = lazyLoad;
                    return context.Exemptions.Include(e=>e.AssignedUsers).Where(e => e.AssignedUsers.Select(u => u.Id).Contains(userId)).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Exemption GetExemption(int exemptionId, bool lazyLoad = true)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    context.Configuration.LazyLoadingEnabled = lazyLoad;
                    return context.Exemptions.Include(e=>e.AssignedUsers).Where(e => e.ID == exemptionId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Exemption> GetUnassignedExemptions()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    Mapper.CreateMap<Models.QuedExemption, Models.Exemption>()
                        .ForMember(dest => dest.QuedExemptionId, opt => opt.MapFrom(src => src.ID));

                    var exemptions = context.Exemptions.Include(e=>e.AssignedUsers)
                        .Where(e => e.AssignedUsers.Count <= 0).ToList() ?? new List<Exemption>();

                    var quedExemptions = GetQuedExemptions()
                        .Select(qe => Mapper.Map<QuedExemption, Exemption>(qe))
                        .ToList() ?? new List<Exemption>();

                    quedExemptions.RemoveAll(qe => context.Exemptions.Any(e => e.QuedExemptionId == qe.ID));

                    exemptions.AddRange(quedExemptions);
                    return exemptions;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Exemption> GetAllExemptions(bool lazyLoad = true)
        {
            try
            {
                Mapper.CreateMap<Models.QuedExemption, Models.Exemption>()
                    .ForMember(dest => dest.QuedExemptionId, opt => opt.MapFrom(src => src.ID));

                var exemptions = GetExemptions(lazyLoad);
                var quedExemptions = GetQuedExemptions()
                    .Select(qe => Mapper.Map<QuedExemption, Exemption>(qe))                    
                    .ToList() ?? new List<Exemption>();
                
                quedExemptions.RemoveAll(qe=>exemptions.Any(e=>e.QuedExemptionId == qe.ID));
                exemptions.AddRange(quedExemptions);                

                return exemptions.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static QuedExemption GetQuedExemptionById(int exemptionId)
        {
            try
            {
                using (QuedExemptionsContext context = new QuedExemptionsContext())
                {
                    return context.QuedExemptions.Find(exemptionId);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool DeleteExemption(Exemption exemption)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var e = context.Exemptions.Find(exemption.ID);
                    e.AssignedUsers.Clear();
                    context.SaveChanges();
                }
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var e = context.Exemptions.Find(exemption.ID);
                    context.Exemptions.Attach(e);
                    context.Exemptions.Remove(e);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool AddExemption(Exemption exemption)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                        foreach (var user in exemption.AssignedUsers)
                            context.Users.Attach(user);
                        context.Exemptions.Add(exemption);                        
                        context.SaveChanges();
                        return true;
                    }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool AssignExemption(Exemption exemption)
        {
            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                var e = context.Exemptions.Find(exemption.ID);
                //Add Exemption
                if (e == null)
                    return AddExemption(exemption);

  
            }
            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                var e = context.Exemptions.Include(ex=>ex.AssignedUsers).FirstOrDefault(ex=>ex.ID == exemption.ID);
                e.AssignedUsers.Clear();
                context.Exemptions.Attach(e);
           //     context.Entry(e).Property(x => x.AssignedUsers).IsModified = true;
                context.SaveChanges();
               
            }
            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                var e = context.Exemptions.Include(ex => ex.AssignedUsers).FirstOrDefault(ex => ex.ID == exemption.ID);
                foreach (var user in exemption.AssignedUsers)
                {
                    context.Users.Attach(user);
                    e.AssignedUsers.Add(user);
                }
                context.Exemptions.Attach(e);
                //     context.Entry(e).Property(x => x.AssignedUsers).IsModified = true;
                context.SaveChanges();

            }
            return true;
        }
        public static bool SetExemptionStatus(int quedExemptionId, ExemptionStatus exemptionStatus)
        {
            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                var e = context.Exemptions.FirstOrDefault(ex => ex.QuedExemptionId == quedExemptionId);
                //Add Exemption
                if (e == null)
                {
                    Mapper.CreateMap<Models.QuedExemption, Models.Exemption>()
                        .ForMember(dest => dest.QuedExemptionId, opt => opt.MapFrom(src => src.ID));
                    var exemption = Mapper.Map<QuedExemption, Exemption>(GetQuedExemptionById(quedExemptionId));
                    exemption.Status = exemptionStatus;
                    return AddExemption(exemption);
                }
            }
            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                var e = context.Exemptions.FirstOrDefault(ex => ex.QuedExemptionId == quedExemptionId);
                e.Status = exemptionStatus;
                context.Exemptions.Attach(e);  
                context.Entry(e).Property(x => x.Status).IsModified = true;
              
                context.SaveChanges();
            }
            return true;
        }

        public static bool AddUpdateExemption(Exemption exemption)
        {
            try
            {                
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var e = context.Exemptions.Find(exemption.ID);
                    //Add Exemption
                    if(e == null)
                        return AddExemption(exemption);

                        e.AssignedUsers.Clear();
                        context.SaveChanges();
                }
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var e = context.Exemptions.Find(exemption.ID);
                    e.AssignedUsers = exemption.AssignedUsers;
                    context.SaveChanges();
                    return true;
                }
            }            
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Setting GetAllSettings()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Set<Setting>().FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
 

        public static byte[] GetDefalutAvatarImage()
        {
            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                var dav = context.Settings.FirstOrDefault(x => x.SettingKey == "DefalutAvatarImage");
                if (dav != null)
                {
                    return Convert.FromBase64String(dav.SettingValue);
                }
            }
            return new byte[0];
        }
    }
}