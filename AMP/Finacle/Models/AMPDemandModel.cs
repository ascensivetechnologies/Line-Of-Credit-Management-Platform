using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class AMPDemandModel
    {
        public int Id { get; set; }
        public string FORACID { get; set; }
        public string AccountName { get; set; }
        public string DemandFlowId { get; set; }
        public decimal DemandAmount { get; set; }
        public DateTime? LastAdjustmentDate { get; set; }
        public decimal TotalAdjustmentAmount { get; set; }
        public DateTime DemandEffectiveDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}