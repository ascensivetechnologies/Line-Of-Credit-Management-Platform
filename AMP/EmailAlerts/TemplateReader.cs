using AMP.Services;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace AMP.EmailAlerts
{
    public class TemplateReader
    {
        public static string ReadHtmlTemplate(string ruleName)
        {
            var mapper = new Dashboard2ServiceLayer().GetAllStringMappers(ruleName).FirstOrDefault();
            if(mapper != null)
            {
                var path = HostingEnvironment.MapPath(@"~/App_Data/EmailTemplates");
                using (StreamReader sr = new StreamReader(Path.Combine(path, mapper.Value)))
                {
                    var templateString = sr.ReadToEnd();
                    return templateString;
                }
            }
            else
            {
                return "";
            }

        }

        public static string GetHtmlTemplatePath(string ruleName)
        {
            var mapper = new Dashboard2ServiceLayer().GetAllStringMappers(ruleName).FirstOrDefault();
            if (mapper != null)
            {
                var path = HostingEnvironment.MapPath(@"~/App_Data/EmailTemplates");
                var templatePath = Path.Combine(path, mapper.Value);
                return templatePath;
            }
            else
            {
                return "";
            }

        }
    }
}