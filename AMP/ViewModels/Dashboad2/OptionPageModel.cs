

using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace AMP.ViewModels.Dashboad2
{
    public class OptionPageModel
    {
        public GridModel<Options> Model { get; set; }
        public string SelectedOption { get; set; }

        public List<SelectListItem> GetOptionList { get; set; }
    }
}