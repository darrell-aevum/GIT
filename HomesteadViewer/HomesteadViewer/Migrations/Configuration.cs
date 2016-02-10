namespace HomesteadViewer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HomesteadViewer.Models;
    using HomesteadViewer.ViewModels.Administration;
    using System.Reflection;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<HomesteadViewer.DAL.HomesteadViewerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HomesteadViewer.DAL.HomesteadViewerContext context)
        {
            if (!context.Settings.Any())
            {
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "Application", SettingKey = "fully qualified domain name", SettingValue = "" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "Application", SettingKey = "data source", SettingValue = @"(localdb)\Projects" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "Application", SettingKey = "catalog", SettingValue = "MasterHS" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "FileRepository", SettingKey = "file repository path", SettingValue = @"c:\" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "FileRepository", SettingKey = "search term column", SettingValue = "QuickRefId" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "General", SettingKey = "site title", SettingValue = "Homestead Viewer" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "General", SettingKey = "anyone can register", SettingValue = "true" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "General", SettingKey = "approval for access", SettingValue = "false" });
                context.Settings.AddOrUpdate(new Setting_DTO { SettingGroup = "General", SettingKey = "only admin can assign", SettingValue = "false" });
            }
            if (!context.GridColumns.Any())
            {
                var model = new GridSettingsViewModel();
                var gcs = new List<GridColumn_DTO>();

                foreach (var prop in model.GetType().GetProperties())
                {
                    var gc = new GridColumn_DTO
                    {
                        ColumnNumber = 0,
                        Displayed = false,
                        DisplayName = prop.Name,
                        Width = null,
                        PropertyName = prop.Name,
                        Editable = false
                    };

                    switch (prop.Name)
                    {
                        case "QuickrefID":
                            gc.Displayed = true;
                            gc.DisplayName = "Quick Ref ID";
                            gc.ColumnNumber = 1;
                            break;
                        case "Status":
                            gc.Displayed = true;
                            gc.DisplayName = "Status";
                            gc.Editable = true;
                            gc.ColumnNumber = 2;
                            break;
                        case "AssignedUser":
                            gc.Displayed = true;
                            gc.DisplayName = "Assigned User";
                            gc.Editable = true;
                            gc.ColumnNumber = 3;
                            break;
                        case "Created":
                            gc.Displayed = true;
                            gc.ColumnNumber = 4;
                            break;
                        case "Modified":
                            gc.Displayed = true;
                            gc.ColumnNumber = 5;
                            break;
                        case "ModifiedBy":
                            gc.Displayed = true;
                            gc.DisplayName = "Modified By";
                            gc.ColumnNumber = 6;
                            break;
                        case "FollowUpDate":
                            gc.Displayed = false;
                            gc.DisplayName = "Follow Up Date";
                            gc.ColumnNumber = 7;
                            gc.Editable = true;
                            break;
                        case "Comment":
                            gc.Displayed = false;
                            gc.DisplayName = "Comment";
                            gc.ColumnNumber = 8;
                            gc.Editable = true;
                            break;
                        default:
                            break;
                    }
                    gcs.Add(gc);
                }

                var c = gcs.Where(g => g.Displayed).Count();
                foreach (var gc in gcs.Where(g => !g.Displayed))
                {
                    gc.ColumnNumber = c + 1;
                    c++;
                }

                foreach (var gc in gcs)
                {
                    if (!context.GridColumns.Any(x=>x.PropertyName == gc.PropertyName))
                    {
                        context.GridColumns.AddOrUpdate(gc);
                    }
                }
            }
        }
    }
}
