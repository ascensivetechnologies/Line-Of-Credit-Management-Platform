using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string LogTime { get; set; }
        public string Month { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentURL { get; set; }
        public string Image { get; set; }
    }
}