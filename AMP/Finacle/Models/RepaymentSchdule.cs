using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class RepaymentSchedule
    {
        public int Id { get; set; }
        public string FORACID { get; set; }
        public string LimitPrefix { get; set; }
        public string AccountName { get; set; }
        public DateTime FlowStart { get; set; }
        public string FlowId { get; set; }
        public decimal FlowAmount { get; set; }
        public string Currency { get; set; }

    }
}