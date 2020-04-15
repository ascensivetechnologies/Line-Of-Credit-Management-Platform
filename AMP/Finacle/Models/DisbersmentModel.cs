using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class DisbersmentModel
    {
        public int Id { get; set; }
        public string ACID { get; set; }
        public string LimitPrefix { get; set; }
        public string LimitB2KID { get; set; }
        public string DisbSerialNo { get; set; }
        public string FORACID { get; set; }
        public string AccountName { get; set; }
        public decimal SanctionLimit { get; set; }
        public decimal DisbAmount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? DisDate { get; set; }

    }
}