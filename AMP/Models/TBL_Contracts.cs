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
    
    public partial class TBL_Contracts
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_Contracts()
        {
            this.TBL_LOC_Contract = new HashSet<TBL_LOC_Contract>();
            this.TBL_ContractContent = new HashSet<TBL_ContractContent>();
            this.TBL_ContractLC = new HashSet<TBL_ContractLC>();
            this.TBL_ContractLocMap = new HashSet<TBL_ContractLocMap>();
            this.TBL_ContractResponsibility = new HashSet<TBL_ContractResponsibility>();
            this.TBL_ContractTerms = new HashSet<TBL_ContractTerms>();
        }
    
        public int Id { get; set; }
        public int PQId { get; set; }
        public string ContractorCIF { get; set; }
        public string ContractorName { get; set; }
        public string CGSId { get; set; }
        public string ContractType { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> TenderIssueDate { get; set; }
        public Nullable<System.DateTime> TenderLastDate { get; set; }
        public Nullable<System.DateTime> PACDate { get; set; }
        public Nullable<System.DateTime> DefectsLiabilityEndDate { get; set; }
        public Nullable<System.DateTime> FACDate { get; set; }
        public Nullable<System.DateTime> CompReportDate { get; set; }
        public Nullable<decimal> AdvPmtGrntAmount { get; set; }
        public Nullable<System.DateTime> AdvPmtGrntExpiry { get; set; }
        public Nullable<decimal> PerBankGrntAmount { get; set; }
        public Nullable<System.DateTime> PerBankGrntExpiry { get; set; }
        public Nullable<decimal> OtherGrntAmount { get; set; }
        public Nullable<System.DateTime> OtherGrntExpiry { get; set; }
        public string PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageDisplayId { get; set; }
        public Nullable<decimal> EstimateValue { get; set; }
        public string TypeOfPackage { get; set; }
        public Nullable<decimal> LocAmount { get; set; }
        public Nullable<decimal> GovtAmount { get; set; }
        public Nullable<decimal> OtherAmount { get; set; }
        public string GuaranteeNote { get; set; }
        public Nullable<System.DateTime> ContractStart { get; set; }
        public Nullable<System.DateTime> ContractEnd { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> ScheduledCompDate { get; set; }
        public Nullable<System.DateTime> ContractApprovalDate { get; set; }
        public Nullable<System.DateTime> RevisedCompletionDate { get; set; }
        public Nullable<System.DateTime> ActualCompletionDate { get; set; }
        public Nullable<System.DateTime> TerminalDateOfDisbursement { get; set; }
        public Nullable<System.DateTime> DateOfReceiptOfContractByEximBank { get; set; }
        public Nullable<System.DateTime> SigningDate { get; set; }
        public Nullable<System.DateTime> SignEffectiveDate { get; set; }
        public Nullable<int> DurationYear { get; set; }
        public Nullable<int> DurationMonth { get; set; }
        public Nullable<int> DurationDay { get; set; }
    
        public virtual TBL_Projects_PQ TBL_Projects_PQ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_LOC_Contract> TBL_LOC_Contract { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_ContractContent> TBL_ContractContent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_ContractLC> TBL_ContractLC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_ContractLocMap> TBL_ContractLocMap { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_ContractResponsibility> TBL_ContractResponsibility { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_ContractTerms> TBL_ContractTerms { get; set; }
    }
}
