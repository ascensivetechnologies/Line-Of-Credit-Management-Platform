using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class SyncModel
    {
        public int Id { get; set; }
        public string System { get; set; }
        public string Status { get; set; }
        public string ServiceName { get; set; }
        public string FullMessage { get; set; }
        public string CreatedOn { get; set; }
        
    }
}