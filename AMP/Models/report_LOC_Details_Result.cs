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
    
    public partial class report_LOC_Details_Result
    {
        public string Approval_Year { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Borrower { get; set; }
        public Nullable<System.DateTime> DEA_Approval_Date { get; set; }
        public Nullable<System.DateTime> MEA_Approval_Date { get; set; }
        public Nullable<decimal> LOC_Amount { get; set; }
        public string Purpose_of_LOC { get; set; }
        public Nullable<System.DateTime> Signing_Date { get; set; }
        public Nullable<decimal> Contracts_Value { get; set; }
        public Nullable<decimal> Total_Disbursements { get; set; }
        public Nullable<decimal> Total_Repayments { get; set; }
        public string Status { get; set; }
    }
}
