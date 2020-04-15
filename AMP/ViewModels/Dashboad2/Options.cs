using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class Options : MasterModel
    {
        public int Id { get; set; }
        public OptionTypes Type { get; set; }
        
        public string Value { get; set; }

        public int? ParentId { get; set; }
        public Options Parent { get; set; }
        public string FaIcon { get; set; }

        public List<SelectListItem> Parents { get; set; }
    }
}