using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web; 
using System.DirectoryServices;
using HomesteadViewer.Models;
using HomesteadViewer.DAL;

namespace HomesteadViewer.ViewModels.AccountModels
{
    public class LoginModel
    {
        public int UserId { get; set; }
        public bool IsAdminUser { get; set; }
        public bool IsNewInstall { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public string FullyQualifiedDomain { get; set; }

        public string ResetToken { get; set; }

        public bool NeedToResetPassword { get; set; }

        public LoginModel()
        {
            IsNewInstall = (User_DTO.Count() <= 0); 
        }


        public bool ValidateLogin()
        {
            if (HttpContext.Current.IsDebuggingEnabled)
                return LocalAccess(); 

            return DomainAccess();
        }

        public bool DomainAccess()
        { 
            if (!string.IsNullOrEmpty(AppSettings.FullyQualifiedDomainName))
            {
                this.FullyQualifiedDomain = AppSettings.FullyQualifiedDomainName;
            }

            if (string.IsNullOrEmpty(this.FullyQualifiedDomain))
                return false;

            string ldap = "LDAP://";
            for (int i = 0; i < FullyQualifiedDomain.Split('.').Count(); i++)
            {
                if (i == FullyQualifiedDomain.Split('.').Count() - 1)
                    ldap = ldap + "DC=" + FullyQualifiedDomain.Split('.')[i] ;
                else
                ldap = ldap + "DC=" + FullyQualifiedDomain.Split('.')[i] + ",";
            } 
            using (var entry = new DirectoryEntry(ldap, UserName, Password))
            {
                try
                {
                   
                    object obj = entry.NativeObject;

                    DirectorySearcher search = new DirectorySearcher(entry);

                    search.Filter = "(SAMAccountName=" + UserName + ")";
                    search.PropertiesToLoad.Add("givenName");
                    search.PropertiesToLoad.Add("sn");
                    search.PropertiesToLoad.Add("mail");
                    SearchResult result = search.FindOne();

                    if (result == null)
                        return false;


                    var user = User_DTO.Get(UserName);
                    if (user == null)
                    {
                        user = new User_DTO();
                        user.UserName = UserName;
                        user.EmailAddress = result.Properties["mail"].Count > 0 ? result.Properties["mail"][0].ToString() : "Unknown";
                        user.FirstName = result.Properties["givenName"].Count > 0 ? result.Properties["givenName"][0].ToString() : "Unknown";
                        user.LastName = result.Properties["sn"].Count > 0 ? result.Properties["sn"][0].ToString() : "Unknown";
                        user.Department = result.Properties["department"].Count > 0 ? result.Properties["department"][0].ToString() : "Unknown";
                        user.IsAdmin = (User_DTO.Count() <= 0);
                        user.IsActive = true;
                        User_DTO.AddUser(user);
                    }

                    UserId = user.Id;
                    IsAdminUser = user.IsAdmin;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        public bool LocalAccess()
        {
            var path = string.Format("WinNT://{0},computer", Environment.MachineName);
            using (var entry = new DirectoryEntry(path, UserName, Password))
            {
                try
                {                  

                    var exists = false;
                    foreach (DirectoryEntry childEntry in entry.Children)
                        if (childEntry.SchemaClassName == "User")
                            if (childEntry.Name.ToLower() == UserName.ToLower())
                                exists = true;

                    if (!exists)
                        return false;

                    var user = User_DTO.Get(UserName);
                    if (user == null)
                    {
                        user = new User_DTO();
                        user.UserName = UserName;
                        user.EmailAddress = "Unknown";
                        user.FirstName = "Unknown";
                        user.LastName = "Unknown";
                        user.Department = "Unknown";
                        user.IsAdmin = (User_DTO.Count() <= 0);
                        user.IsActive = true;
                        User_DTO.AddUser(user);
                    }

                    UserId = user.Id;
                    IsAdminUser = user.IsAdmin;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
        }
    }
}