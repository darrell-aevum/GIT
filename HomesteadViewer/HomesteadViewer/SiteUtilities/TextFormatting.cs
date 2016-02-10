using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HomesteadViewer.SiteUtilities
{
    public class TextFormatting
    {
        public static string CleanseTextForUrls(string input)
        {
            input = input.Trim().ToLower().Replace("  ", " ").Replace(" ", "-");
            var regex = new Regex("[^0-9a-zA-Z-]+");
            return regex.Replace(input, String.Empty);
        }
    }
}