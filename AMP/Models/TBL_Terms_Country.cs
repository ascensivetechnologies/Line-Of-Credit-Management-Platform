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
    
    public partial class TBL_Terms_Country
    {
        public int Id { get; set; }
        public string RiskCategoryClass { get; set; }
        public string LocGuideLinesClass { get; set; }
        public double InterestRate { get; set; }
        public double CommitmentFee { get; set; }
        public double ManagementFee { get; set; }
        public int Tenure { get; set; }
        public int Moratorium { get; set; }
        public double IndianContribution { get; set; }
        public string SpecialConditions { get; set; }
        public string CurrecyCode { get; set; }
        public int CountryId { get; set; }
        public string ApprovalType { get; set; }
        public string Type { get; set; }
        public double Percentage { get; set; }
        public string InterestType { get; set; }
    
        public virtual TBL_Country TBL_Country { get; set; }
    }
}
