using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class LocDetailsModel
    {
        public int Id { get; set; }
        public string LimitB2KID { get; set; }
        public string AccountName { get; set; }
        public string FreeText { get; set; }
        public string LimitPrefix { get; set; }
        public string LimitSuffix { get; set; }
        public decimal SanctionAmount { get; set; }
        public string CIF { get; set; }
    }
}