using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class PrincipalDueModel
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string DemandType { get; set; }
        public DateTime DueDate { get; set; }
        public string Frequency { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime RunDate { get; set; }
    }
}