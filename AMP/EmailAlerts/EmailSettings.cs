using AMP.Services;
using System;
using System.Linq;

namespace AMP.EmailAlerts
{
    public class EmailSettings
    {
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public int Port { get; set; }
        public string EmailSMTP { get; set; }

        public EmailSettings()
        {
            EmailUserName =  new Dashboard2ServiceLayer().GetAllStringMappers("EmailUserName")
                .DefaultIfEmpty(new ViewModels.Dashboad2.StringMapperModel {Value = "" })
                .FirstOrDefault().Value;
            EmailPassword = new Dashboard2ServiceLayer().GetAllStringMappers("EmailPassword")
                .DefaultIfEmpty(new ViewModels.Dashboad2.StringMapperModel { Value = "" })
                .FirstOrDefault().Value;
            try
            {
                Port = Convert.ToInt32(new Dashboard2ServiceLayer().GetAllStringMappers("EmailPort")
                .DefaultIfEmpty(new ViewModels.Dashboad2.StringMapperModel { Value = "0" })
                .FirstOrDefault().Value);
            }
            catch
            {
                Port = 0;
            }
            EmailSMTP = new Dashboard2ServiceLayer().GetAllStringMappers("EmailSMTP")
                .DefaultIfEmpty(new ViewModels.Dashboad2.StringMapperModel { Value = "" })
                .FirstOrDefault().Value;
        }
    }
}