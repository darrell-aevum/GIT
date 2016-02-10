using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Humanizer;

namespace HomesteadViewer.Helpers
{
    public static class KendoTemplateHelper
    {
        public static IHtmlString KendoTemplateDetail<T>(this HtmlHelper helper, string name) where T : class
        {
            var html = new StringBuilder();
            html.AppendFormat("<script id=\"{0}\" type=\"text/kendo-tmpl\">\n", name);
            html.AppendLine("<dl class=\"dl-horizontal\">");
            foreach (var p in typeof (T).GetProperties())
            {
                html.AppendFormat("<dt>{0}</dt>", p.Name.Titleize());
                html.AppendFormat("<dd>#= {0} #&nbsp;</dd>", p.Name);
            }
            html.AppendLine("</dl>");
            html.AppendLine("</script>");

            return MvcHtmlString.Create(html.ToString());
        }
    }
}