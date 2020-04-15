using AMP.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class MasterModel
    {
        public LmpPrincipal User { get
            {
                return UserConversion.Convertuser();
            }
        }

    }
}