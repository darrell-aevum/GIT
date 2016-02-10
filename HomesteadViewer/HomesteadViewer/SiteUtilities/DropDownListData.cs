using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomesteadViewer.Models;

namespace HomesteadViewer.SiteUtilities
{
    public static class DropDownListData
    {
        public static SelectList Users() { return GetUsers(); }  

        private static SelectList GetUsers()
        {
            var users = User_DTO.GetAll();
            var sl = new SelectList(users, "Id", "FullName");
            return sl;
        } 
    }
}