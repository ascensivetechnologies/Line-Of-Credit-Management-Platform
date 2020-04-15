using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class LocTransactions
    {
        public int Id { get; set; }
        public string FORACID { get; set; }
        public string CIFID { get; set; }
        public string DemandFlow { get; set; }
        public string DeleteFlag { get; set; }
        public decimal? DemandAmount { get; set; }
        public DateTime? LastAdjustmentDate { get; set; }
        public decimal? TotalAdjustedAmount { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CurrencyCode { get; set; }
        public string Prefix { get; set; }
    }
}