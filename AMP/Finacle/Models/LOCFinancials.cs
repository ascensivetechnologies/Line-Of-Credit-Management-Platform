using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class LOCFinancials
    {
        public int Id { get; set; }
        public string LimitPrefix { get; set; }
        public string FORACID { get; set; }
        public string AccountName { get; set; }
        public decimal LoanOutstanding { get; set; }
        public decimal TotalDisbursed { get; set; }
        public decimal PrincipalDemand { get; set; }
        public decimal PrincipalCollection { get; set; }
        public decimal PrincipalOverdue { get; set; }
        public decimal InterestDemand { get; set; }
        public decimal InterestCollection { get; set; }
        public decimal InterestOverdue { get; set; }
    }
}