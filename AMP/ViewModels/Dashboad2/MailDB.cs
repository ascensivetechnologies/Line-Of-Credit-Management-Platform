using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class MailDB
    {
        public int Id { get; set; }
        public string MailSubject { get; set; }
        public string Body { get; set; }
        public string SentDate { get; set; }
        public string ToAddress { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Status { get; set; }
    }
}