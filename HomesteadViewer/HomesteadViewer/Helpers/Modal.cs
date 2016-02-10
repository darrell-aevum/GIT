using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HomesteadViewer.Helpers
{
    public static class ModalHelper
    {
        public static IDisposable BeginModal(this HtmlHelper helper, string id, ElementSize size = ElementSize.Normal)
        {
            var modalSize = String.Empty;
            if (size == ElementSize.ExtraSmall || size == ElementSize.Small)
                modalSize = " modal-sm";

            if (size == ElementSize.Large)
                modalSize = " modal-lg";

            helper.ViewContext.Writer.WriteLine("<div class=\"modal fade\" id=\"{0}\" tabindex=\"-1\" role=\"dialog\">", id);
            helper.ViewContext.Writer.WriteLine("<div class=\"modal-dialog{0}\">", modalSize);
            helper.ViewContext.Writer.WriteLine("<div class=\"modal-content\">");
        
            return new Modal(helper);
        }

        public static void ModalHeader(this HtmlHelper helper, string title)
        {
            helper.ViewContext.Writer.WriteLine("<div class=\"modal-header\">");
            helper.ViewContext.Writer.WriteLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">Close</span></button>");
            helper.ViewContext.Writer.WriteLine("<h4 class=\"modal-title\">{0}</h4>", title);
            helper.ViewContext.Writer.WriteLine("</div>");
        }

        public static IDisposable BeginModalBody(this HtmlHelper helper)
        {
            helper.ViewContext.Writer.WriteLine("<div class=\"modal-body\">");
            return new ModalSection(helper);
        }

        public static IDisposable BeginModalFooter(this HtmlHelper helper)
        {
            helper.ViewContext.Writer.WriteLine("<div class=\"modal-footer\">");
            return new ModalSection(helper);
        }

        class Modal : IDisposable
        {
            private HtmlHelper helper;

            public Modal(HtmlHelper helper)
            {
                this.helper = helper;
            }

            public void Dispose()
            {
                this.helper.ViewContext.Writer.WriteLine("</div>");
                this.helper.ViewContext.Writer.WriteLine("</div>");
                this.helper.ViewContext.Writer.WriteLine("</div>");
            }
        }

        class ModalSection : IDisposable
        {
            private HtmlHelper helper;

            public ModalSection(HtmlHelper helper)
            {
                this.helper = helper;
            }

            public void Dispose()
            {
                this.helper.ViewContext.Writer.Write("</div>");
            }
        }
    }

    public enum ElementSize
    {
        ExtraSmall,
        Small,
        Normal,
        Large
    }
}

