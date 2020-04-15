using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class ContractTransaction
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string CIF { get; set; }
        public DateTime? TranDate { get; set; }
        public string TranId { get; set; }
        public decimal TranAmount { get; set; }
        public string Particulars { get; set; }
        public string Currency { get; set; }
        public decimal SanctionedAmount { get; set; }
        public decimal CummulativeDebit { get; set; }
        public decimal CummulativeCredit { get; set; }
    }
}