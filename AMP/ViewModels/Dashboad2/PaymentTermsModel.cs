using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class PaymentTermsModel
    {
        public int Id { get; set; }
        public int? Sequence { get; set; }
        public int ContractId { get; set; }
        public string Milestone { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Amount { get; set; }
        public string Note { get; set; }
        
    }
}