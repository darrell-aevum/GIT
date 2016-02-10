using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace HomesteadViewer.Models
{
    [Table("Settings")]    
    public class Setting
    {
        [Key, Column(Order = 0), StringLength(50)]
        public string SettingGroup { get; set; }
        [Key, Column(Order = 1), StringLength(50)]
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }
    }
    //public class Setting
    //{
    //    [Key]
    //    public int Id { get; set; }

    //    public string FileRepository_Path { get; set; }
    //    public NamingConventionPropertyOptioin FileRepository_NamingConventionPropertyOptioin { get; set; }

    //    public IEnumerable<string> Network_Domains { get; set; }

    //    public bool General_AnyoneCanRegister { get; set; }
    //    public bool General_RequireApprovalForAccess { get; set; }
    //    public bool General_RequireAdminToAssign { get; set; }
    //    public string General_SiteTitle { get; set; } 
    //    public byte[] DefalutAvatarImage { get; set; } 
    //}
}