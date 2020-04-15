using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class CGSModel
    {
        public int Id { get; set; }
        public string FORACID { get; set; }
        public string AccountName { get; set; }
        public DateTime AccountOpenDate { get; set; }
        public decimal ClearBalanceAmount { get; set; }
        public string AccountMgr { get; set; }
        public string CIF { get; set; }
        public string SchemeCode { get; set; }
        public string Currency { get; set; }
    }
}