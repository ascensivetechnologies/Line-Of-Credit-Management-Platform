using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.DashboardModels
{
    public class DashboardLocModel
    {
        public string Name { get; set; }
        public decimal Allocated { get; set; }
        public decimal Disbursed { get; set; }
        public decimal Percentage { get; set; }
    }
}