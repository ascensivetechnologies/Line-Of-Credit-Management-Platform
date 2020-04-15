using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class StringMapperModel : MasterModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
       
    }


}