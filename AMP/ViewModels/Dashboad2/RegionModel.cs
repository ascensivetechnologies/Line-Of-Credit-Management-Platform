using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class RegionModel : MasterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddedBy { get; set; }
        public string AddedOn { get; set; }
        public int Countries { get; set; }
    }

    
}