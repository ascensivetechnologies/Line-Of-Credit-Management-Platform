using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class CreditLetterModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string LCNumber { get; set; }
        public string Beneficiary { get; set; }
        public decimal? Amount { get; set; }
        public String OpeningDate { get; set; }
        public string IssuingBank { get; set; }
        public String LastDateofShipment { get; set; }
        public String ExpiryDate { get; set; }
        public decimal? TransferableAmount { get; set; }
        public string LCNote { get; set; }
    }
}