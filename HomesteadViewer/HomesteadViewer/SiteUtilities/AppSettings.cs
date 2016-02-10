using System;
using System.Linq;
using HomesteadViewer.Models;

namespace HomesteadViewer
{
    public static class AppSettings
    { 
        public static string FullyQualifiedDomainName
        {
            get
            {
                return Setting_DTO.GetSetting("fully qualified domain name").SettingValue ?? "";
            }
            set
            {
                var setting = Setting_DTO.GetSetting("fully qualified domain name");
                setting.SettingValue = value ?? "";
                Setting_DTO.UpdateSetting(setting);
            }
        }
        public static string ConnectionString
        {
            get
            {
                if(string.IsNullOrEmpty(DataSource) || string.IsNullOrEmpty(Catalog))
                    return "";
                return "Data Source=" + DataSource + ";Initial Catalog=" + Catalog + ";Integrated Security=SSPI;";
            } 
        }

        public static string DataSource
        {
            get
            {
                return Setting_DTO.GetSetting("data source").SettingValue ?? "";
            }
            set
            {
                var setting = Setting_DTO.GetSetting("data source");
                setting.SettingValue = value ?? "";
                Setting_DTO.UpdateSetting(setting);
            }
        }

        public static string Catalog
        {
            get
            {
                return Setting_DTO.GetSetting("catalog").SettingValue ?? "";
            }
            set
            {
                var setting = Setting_DTO.GetSetting("catalog");
                setting.SettingValue = value ?? "";
                Setting_DTO.UpdateSetting(setting);
            }
        }


        public static string FileRepositoryPath
        {
            get
            {
                return Setting_DTO.GetSetting("file repository path").SettingValue ?? "";
            }
            set
            {
                var setting = Setting_DTO.GetSetting("file repository path");
                setting.SettingValue = value ?? "";
                Setting_DTO.UpdateSetting(setting);
            }
        }

        public static string SearchTermTableColumn
        {
            get
            {
                return Setting_DTO.GetSetting("search term column").SettingValue ?? "";
            }
            set
            {
                var setting = Setting_DTO.GetSetting("search term column");
                setting.SettingValue = value ?? "";
                Setting_DTO.UpdateSetting(setting);
            }
        }


        public static string SiteTitle
        {
            get
            {
                return Setting_DTO.GetSetting("site title").SettingValue ?? "";
            }
            set
            {
                var setting = Setting_DTO.GetSetting("site title");
                setting.SettingValue = value ?? "";
                Setting_DTO.UpdateSetting(setting);
            }
        }


        public static bool AnyoneCanRegister
        {
            get
            {
                bool val = true;
                bool.TryParse(Setting_DTO.GetSetting("anyone can register").SettingValue, out val);
                return val;
            }
            set
            {
                var setting = Setting_DTO.GetSetting("anyone can register");
                setting.SettingValue = value.ToString();
                Setting_DTO.UpdateSetting(setting);
            }
        }

        public static bool ApprovalForAccess
        {
            get
            {
                bool val = true;
                bool.TryParse(Setting_DTO.GetSetting("approval for access").SettingValue, out val);
                return val;
            }
            set
            {
                var setting = Setting_DTO.GetSetting("approval for access");
                setting.SettingValue = value.ToString();
                Setting_DTO.UpdateSetting(setting);
            }
        }

        public static bool OnlyAdminCanAssign
        {
            get
            {
                bool val = true;
                bool.TryParse(Setting_DTO.GetSetting("only admin can assign").SettingValue, out val);
                return val;
            }
            set
            {
                var setting = Setting_DTO.GetSetting("only admin can assign");
                setting.SettingValue = value.ToString();
                Setting_DTO.UpdateSetting(setting);
            }
        }
    }
}