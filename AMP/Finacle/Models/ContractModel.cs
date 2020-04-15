using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class ContractModel
    {
        public int Id { get; set; }
        public string CIF { get; set; }
        public string ContractId { get; set; }
        public string ContractorName { get; set; }
        public decimal ContractValue { get; set; }
        public DateTime ContractDate { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string CGS_ID { get; set; }
    }
}