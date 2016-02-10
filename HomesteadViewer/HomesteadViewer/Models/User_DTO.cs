using AutoMapper;
using HomesteadViewer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web.Script.Serialization; 
namespace HomesteadViewer.Models
{
    [Table("Users")]
    public class User_DTO
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50), Index(IsUnique = true)]
        public string UserName { get; set; }

        [Required, StringLength(250)]
        public string EmailAddress { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; } 

        [StringLength(12)]
        public string Phone { get; set; }

        [StringLength(12)]
        public string MobileNumber { get; set; }

        [StringLength(50)]
        public string JobTitle { get; set; }

        [StringLength(5)]
        public string Extension { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }

        [NotMapped]
        public virtual List<AdministrativeProperties_DTO> AssignedExemptions {
            get {
                return AdministrativeProperties_DTO.GetAll(x => x.AssignedUserID == this.Id).ToList() ?? new List<AdministrativeProperties_DTO>();
            }
        }

        public User_DTO()
        {
            
        }
        
        public static User_DTO Get(int id)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.Where(u => u.Id == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static User_DTO Get(string userName)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.FirstOrDefault(c => c.UserName == userName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }  
        public static User_DTO Get(Func<User_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.Where(func).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public static List<User_DTO> GetAll()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<User_DTO> GetAll(Func<User_DTO, bool> func)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Users.Where(func).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool Exists(int userId)
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.Users.Any(u=>u.Id == userId);
            }
        }
        public static bool Exists(string userName)
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.Users.Any(u => u.UserName == userName);
            }
        }
        public static bool Exists(Func<User_DTO, bool> func)
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.Users.Any(func);
            }
        }

        public static int Count()
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.Users.Count();
            }
        }
        public static int Count(Func<User_DTO, bool> func)
        {
            using (var context = new HomesteadViewerContext())
            {
                return context.Users.Count(func);
            }
        }

        public static bool UpdateUser(int id, string firstName, string lastName, string phone, string email)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var user = Get(id);
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
        public static User_DTO UpdateUser(User_DTO user)
        {
            Mapper.CreateMap<Models.User_DTO, Models.User_DTO>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                try
                {
                    var c = Get(user.Id);
                    if (c == null)
                        throw new Exception("User Could Not Be Found");
                    Mapper.Map<Models.User_DTO, Models.User_DTO>(user, c);

                    context.Users.Attach(c);
                    context.Entry(c).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return Get(user.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        } 
        public static bool AddUser(string userName, string phone, string email)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    User_DTO user = new User_DTO()
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
        public static bool AddUser(User_DTO user)
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
        public static bool DeleteUser(int id)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var user = Get(id);

                    if (user == null)
                        return false;

                    //                    foreach (var ae in GetAssignedExemptionsByUserId(user.Id))
                    //                      UnassignExemption(ae.OnlineExemptionID);

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
    }
}