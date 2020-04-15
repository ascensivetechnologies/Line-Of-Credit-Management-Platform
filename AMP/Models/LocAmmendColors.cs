using System;
using System.ComponentModel.DataAnnotations;

namespace AMP.Models
{
    public class LocAmmendColors
    {
        public int Id { get; set; }
        public Nullable<int> LOC_Id { get; set; }
        public int CompId { get; set; }
        public string AuditDate { get; set; }
        public string AmendedBy { get; set; }
        public string AgreementAmendmentDate { get; set; }
        public string TerminalDate { get; set; }
        public string IndianContribution { get; set; }
        public string MdAppDate { get; set; }
        public string DEAAppDate { get; set; }
        public string MEAAppDate { get; set; }
        public string OfferLetterDate { get; set; }
        public string InterestRate { get; set; }
        public string CommitmentFee { get; set; }
        public string ManagementFee { get; set; }
        public string SigningDate { get; set; }
        public string InterestEqualization { get; set; }
        public string Type { get; set; }
        public string Percentage { get; set; }
        public string Tenure { get; set; }
        public string Moratorium { get; set; }
        public string SpecialCondition { get; set; }
        public string OmNumber { get; set; }
        public string LOCPurpose { get; set; }
        public string InterestType { get; set; }
        public string AmendmentNote { get; set; }
        //public string SanctionAmount { get; set; }
        public string AmountAllocated { get; set; }
        public string MEA_Type { get; set; }
        public string MEA_Percentage { get; set; }
        public string DEA_Type { get; set; }
        public string DEA_Percentage { get; set; }
    }
}