//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Finacle_Contract_Transanctions
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string CIF { get; set; }
        public Nullable<System.DateTime> TranDate { get; set; }
        public Nullable<decimal> TranAmount { get; set; }
        public string Particulars { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> SanctionedAmount { get; set; }
        public Nullable<decimal> CummulativeDebit { get; set; }
        public Nullable<decimal> CummulativeCredit { get; set; }
        public string TranId { get; set; }
    }
}