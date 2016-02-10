using AutoMapper;
using HomesteadViewer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HomesteadViewer.SiteUtilities;

namespace HomesteadViewer.Models
{
    [Table("Settings")]
    public class Setting_DTO
    {
        [Key, Column(Order = 0), StringLength(50)]
        public string SettingGroup { get; set; }
        [Key, Column(Order = 1), StringLength(50)]
        public string SettingKey { get; set; }

        public string SettingValue { get; set; }

        public static List<Setting_DTO> GetAllSettings()
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Settings.ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        }
        public static Setting_DTO GetSetting(string settingKey)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    return context.Settings.FirstOrDefault(x => x.SettingKey == settingKey) ?? new Setting_DTO();
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        } 
        public static Setting_DTO UpdateSetting(string settingGroup, string settingKey, string settingValue)
        {
            try
            {
                using (HomesteadViewerContext context = new HomesteadViewerContext())
                {
                    var setting = GetSetting(settingKey); 
                    if (setting == null)
                        throw new Exception("Setting Does Not Exist.");

                    setting.SettingGroup = (string.IsNullOrEmpty(settingGroup)) ? "" : settingGroup;
                    setting.SettingValue = setting.SettingValue;

                    context.Settings.Attach(setting);
                    context.Entry(setting).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return GetSetting(settingKey);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
                return null;
            }
        } 
        public static Setting_DTO UpdateSetting(Setting_DTO setting)
        {
            Mapper.CreateMap<Models.Setting_DTO, Models.Setting_DTO>();
                

            using (HomesteadViewerContext context = new HomesteadViewerContext())
            {
                try
                {
                    var s = GetSetting(setting.SettingKey);
                    if (s == null)
                        throw new Exception("Setting Does Not Exist");

                    Mapper.Map(setting, s);
                    s.SettingGroup = (string.IsNullOrEmpty(setting.SettingGroup)) ? "" : setting.SettingGroup;
                    context.Settings.Attach(s);
                    context.Entry(s).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return GetSetting(s.SettingKey);
                }
                catch (Exception ex)
                {
                    ex.Log();   
                    return null;
                }
            }
        }
    }
}