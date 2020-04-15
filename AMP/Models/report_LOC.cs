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
    
    public partial class report_LOC
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string OmNumber { get; set; }
        public Nullable<decimal> AmountAllocated { get; set; }
        public Nullable<System.DateTime> TerminalDate { get; set; }
        public Nullable<System.DateTime> MEAAppDate { get; set; }
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<System.DateTime> MdAppDate { get; set; }
        public Nullable<System.DateTime> OfferLetterDate { get; set; }
        public string Purpose { get; set; }
        public double InterestRate { get; set; }
        public double CommitmentFee { get; set; }
        public double ManagementFee { get; set; }
        public Nullable<int> Tenor { get; set; }
        public Nullable<int> Moratorium { get; set; }
        public Nullable<double> IndianContribution { get; set; }
        public string SpecialCondition { get; set; }
        public string LocAccountNo { get; set; }
        public Nullable<System.DateTime> SigningDate { get; set; }
        public Nullable<System.DateTime> AgreementAmendmentDate { get; set; }
        public string MEA_Type { get; set; }
        public Nullable<double> MEA_Percentage { get; set; }
        public string DEA_Type { get; set; }
        public Nullable<double> DEA_Percentage { get; set; }
        public string LocNumber { get; set; }
        public string Classification { get; set; }
        public Nullable<System.DateTime> OM_Date { get; set; }
        public string LOC_Status { get; set; }
        public string Interest_Type { get; set; }
        public decimal Sanctioned_Amount { get; set; }
        public decimal Disbursed_Amount { get; set; }
        public System.DateTime First_Disbursement { get; set; }
        public Nullable<int> ProjectCount { get; set; }
        public decimal Amount_Repaid { get; set; }
        public decimal Amount_Outstanding { get; set; }
        public decimal Principal_Overdue { get; set; }
        public decimal Interest_Overdue { get; set; }
        public System.DateTime Principal_Repayment_Start { get; set; }
        public System.DateTime Principal_Repayment_End { get; set; }
        public string Interest_Due_Dates { get; set; }
        public string Principal_Due_Dates { get; set; }
        public Nullable<System.DateTime> BalanceConfirmationDate { get; set; }
        public string BalanceConfirmationBy { get; set; }
        public string BalanceConfirmationPeriod { get; set; }
    }
}