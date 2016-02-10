using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace HomesteadViewer.Helpers
{
    public static class ListViewExtensions
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class GridStatusAttribute : Attribute
        {
            public string Warning { get; set; }
            public string Danger { get; set; }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class GridGroupAttribute : Attribute
        {

        }

        [AttributeUsage(AttributeTargets.Property)]
        public class GridSortAttribute : Attribute
        {

        }

        public static Kendo.Mvc.UI.Fluent.GridBuilder<T> DefaultListView<T>(this HtmlHelper helper) where T : class
        {
            var hasSelectUrl = typeof(T).GetProperties().Any(x => x.Name == "SelectUrl");
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            return helper.Kendo().Grid<T>()
                .Name(routeData.Values["action"].ToString().ToLower() + "-grid")
                .DataSource(x => x.Ajax().Read(routeData.Values["action"].ToString() + "_Read", routeData.Values["controller"].ToString()))
                .Columns(columns =>
                {
                    foreach (var prop in typeof(T).GetProperties().Where(x => !x.Name.Contains("Status") && !x.Name.Contains("Display") && !x.Name.Contains("Url")))
                    {
                        var hasDisplay = typeof(T).GetProperties().Any(x => x.Name.Contains(prop.Name + "Display"));
                        var hasStatus = typeof(T).GetProperties().Any(x => x.Name.Contains(prop.Name + "Status"));
                        var clientTemplate = String.Empty;
                        if(hasDisplay && !hasStatus)
                            clientTemplate = "#= " + prop.Name + "Display #";
                        else if(!hasDisplay && hasStatus)
                            clientTemplate = "<span class='label label-block label-#= " + prop.Name + "Status # center'>#= " + prop.Name + " #</span>";
                        else if(hasDisplay && hasStatus)
                            clientTemplate = "<span class='label label-block label-#= " + prop.Name + "Status # center'>#= " + prop.Name + "Display #</span>";

                        columns.Bound(prop.Name).ClientTemplate(clientTemplate);
                    }
                })
                .Selectable(x => x.Enabled(hasSelectUrl).Mode(GridSelectionMode.Single).Type(GridSelectionType.Row))
                .Events(events =>
                {
                    if (hasSelectUrl)
                    {
                        events.DataBound("addHoverHighlight");
                        events.Change("selectRowNavigate");
                    }
                })
                .Filterable()
                .Sortable()
                .ColumnMenu();
        }

        public static Kendo.Mvc.UI.Fluent.GridBuilder<T> DefaultListView<T>(this HtmlHelper helper, string apiController) where T : class
        {
            var hasSelectUrl = typeof(T).GetProperties().Any(x => x.Name == "SelectUrl");
            var hasGroup = typeof(T).GetProperties().Any(x => x.Name == "Group");
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            return helper.Kendo().Grid<T>()
                .Name(apiController.ToLower() + "-grid")
                .DataSource(x =>
                {
                    x.Ajax().Read("Read", apiController, new {area = "AjaxApi"});
                    if (hasGroup)
                    {
                        x.Ajax().Group(g => g.Add("Group", typeof(string)));
                    }
                    
                })
                .Columns(columns =>
                {
                    foreach (var prop in typeof(T).GetProperties().Where(x => !x.Name.Contains("Status") && !x.Name.Contains("Display") && !x.Name.Contains("Url")))
                    {
                        var clientTemplate = GetTemplateFromProp(prop);
                        columns.Bound(prop.Name).ClientTemplate(clientTemplate);
                    }
                })
                .Selectable(x => x.Enabled(hasSelectUrl).Mode(GridSelectionMode.Single).Type(GridSelectionType.Row))
                .Events(events =>
                {
                    if (hasSelectUrl)
                    {
                        events.DataBound("addHoverHighlight");
                        events.Change("selectRowNavigate");
                    }
                })
                .Filterable(f => f.Mode(GridFilterMode.Row))
                .Sortable()
                .ColumnMenu();
        }

        public static string GetTemplateFromProp(PropertyInfo prop)
        {
            var warningSetting = GetGridStatusFromProp(prop, "Warning");
            var dangerSetting = GetGridStatusFromProp(prop, "Danger");
            var customDisplayValue = GetDisplayValue(prop);

            if (customDisplayValue != null && warningSetting == null && dangerSetting == null)
                return String.Format("#: {0} #", customDisplayValue);

            if (warningSetting != null && dangerSetting != null)
                return String.Format(GetFullTemplate(), customDisplayValue ?? prop.Name, dangerSetting, warningSetting);

            if (warningSetting == null && dangerSetting != null)
                return String.Format(GetDangerTemplate(), customDisplayValue ?? prop.Name, dangerSetting);

            return null;
        }

        private static string GetDisplayValue(PropertyInfo prop)
        {
            if (prop.PropertyType == typeof (DateTime) || prop.PropertyType == typeof (DateTime?))
            {
                return String.Format("moment({0}).fromNow()", prop.Name);
            }

            if (prop.PropertyType == typeof(bool))
            {
                return String.Format("{0} ? 'YES' : 'NO'", prop.Name);
            }

            return null;
        }

        private static string GetDangerTemplate()
        {
            return @"
# if ({1}) {{ # 
<span class='label label-block label-danger center'>#: {0} #</span> 
# }} else {{ # 
<span class='label label-block label-success center'>#: {0} #</span> 
# }} #";
        }

        private static string GetFullTemplate()
        {
            return @"
# if ({1}) { # 
    <span class='label label-block label-danger center'>#: {0} #</span>
# else if ({2}) { # 
    <span class='label label-block label-warning center'>#: {0} #</span>
# } else { #
    <span class='label label-block label-success center'>#: {0} #</span>
# } #
";
        }

        private static string GetGridStatusFromProp(PropertyInfo prop, string memberName)
        {
            if (!prop.CustomAttributes.Any())
                return null;

            var attr = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(GridStatusAttribute));
            if (attr == null)
                return null;

            var arg = attr.NamedArguments.FirstOrDefault(x => x.MemberName == memberName);
            var setting = arg.MemberInfo == null ? null : arg.TypedValue.Value.ToString();

            if (setting == null) return null;

            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                if(setting.ToLower() == "now")
                    return String.Format("{0} < Date.now()", prop.Name);

                return String.Format("{0} < moment().add({1}, '{2}')", prop.Name, setting.Split(' ')[0], setting.Split(' ')[1].ToLower());
            }

            if (prop.PropertyType == typeof(bool))
            {
                return String.Format("{0} === false", prop.Name);
            }

            if (prop.PropertyType == typeof(string))
            {
                return String.Format("{0} === {1}", prop.Name, setting);
            }

            return null;
        }        
    }
}