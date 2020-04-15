using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class TeamMappingModel : UsersModel
    {
        public int TeamId { get; set; }
        public string Image { get
            {
                return ContactImage != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(ContactImage)) : "";
            }
        }
       
    }
}