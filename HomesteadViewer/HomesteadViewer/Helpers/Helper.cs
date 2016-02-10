using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace HomesteadViewer
{
    public class Helper
    {
        public static Dictionary<string, string> GetAssociatedExemptionFiles(string id){
            try
            {
                var dic = new Dictionary<string, string>();
                var di = new DirectoryInfo(AppSettings.FileRepositoryPath);
                var files = di.GetFiles("*", SearchOption.AllDirectories).Where(f => f.Name.Contains(id)).ToList();
                foreach (var file in files)
                {
                    var filepath = file.FullName.Replace(@"\\", "file://///").Replace(@"\", "/");

                    dic.Add(file.Name, filepath);
                }
                return dic;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, string>();
            }
        }

        public static string SerializeObject(object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
            
        } 
    }
}