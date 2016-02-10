using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HomesteadViewer.Models;

namespace HomesteadViewer.ViewModels.UserModels
{
    [NotMapped]
    public class UserListModel : Models.User_DTO
    {
        public static List<UserListModel> Load()
        {
            var users = User_DTO.GetAllUsers();

            return users.Select(x => new UserListModel
              {
                  Id = x.Id,
                  UserName = x.UserName,
                  Department = x.Department,
                  EmailAddress = x.EmailAddress,
                  FirstName = x.FullName,
                  LastName = x.LastName,
                  IsAdmin = x.IsAdmin,
                  Phone = x.Phone,
                  Extension = x.Extension,
                  JobTitle = x.JobTitle,
                  MobileNumber = x.MobileNumber,
                  IsActive = x.IsActive
              }).OrderByDescending(x => x.LastName).ToList();
        }
    }
}