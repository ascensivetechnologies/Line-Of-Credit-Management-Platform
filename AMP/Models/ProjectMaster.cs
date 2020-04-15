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
    
    public partial class ProjectMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectMaster()
        {
            this.ComponentMasters = new HashSet<ComponentMaster>();
            this.GeoCodes = new HashSet<GeoCode>();
            this.GeoCodes1 = new HashSet<GeoCode>();
            this.AuditedFinancialStatements = new HashSet<AuditedFinancialStatement>();
            this.Evaluations = new HashSet<Evaluation>();
            this.Portfolios = new HashSet<Portfolio>();
            this.ProjectBudgetCentreOrgUnits = new HashSet<ProjectBudgetCentreOrgUnit>();
            this.ProjectOutputs = new HashSet<ProjectOutput>();
            this.Reports = new HashSet<Report>();
            this.ReviewARScores = new HashSet<ReviewARScore>();
            this.ReviewDeferrals = new HashSet<ReviewDeferral>();
            this.ReviewDeferrals1 = new HashSet<ReviewDeferral>();
            this.ReviewExemptions = new HashSet<ReviewExemption>();
            this.ReviewMasters = new HashSet<ReviewMaster>();
            this.ReviewOutputs = new HashSet<ReviewOutput>();
            this.ReviewPCRScores = new HashSet<ReviewPCRScore>();
            this.RiskDocuments = new HashSet<RiskDocument>();
            this.RiskRegisters = new HashSet<RiskRegister>();
            this.Teams = new HashSet<Team>();
            this.TeamExternals = new HashSet<TeamExternal>();
            this.WorkflowMaster1 = new HashSet<WorkflowMaster1>();
        }
    
        public string ProjectID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BudgetCentreID { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComponentMaster> ComponentMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GeoCode> GeoCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GeoCode> GeoCodes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuditedFinancialStatement> AuditedFinancialStatements { get; set; }
        public virtual ConditionalityReview ConditionalityReview { get; set; }
        public virtual DSOMarker DSOMarker { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evaluation> Evaluations { get; set; }
        public virtual Markers1 Markers1 { get; set; }
        public virtual Performance Performance { get; set; }
        public virtual ProjectDate ProjectDate { get; set; }
        public virtual ProjectInfo ProjectInfo { get; set; }
        public virtual Deferral Deferral { get; set; }
        public virtual Deferral Deferral1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio> Portfolios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectBudgetCentreOrgUnit> ProjectBudgetCentreOrgUnits { get; set; }
        public virtual BudgetCentre BudgetCentre { get; set; }
        public virtual Stage Stage1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectOutput> ProjectOutputs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewARScore> ReviewARScores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewDeferral> ReviewDeferrals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewDeferral> ReviewDeferrals1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewExemption> ReviewExemptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewMaster> ReviewMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewOutput> ReviewOutputs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReviewPCRScore> ReviewPCRScores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RiskDocument> RiskDocuments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RiskRegister> RiskRegisters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team> Teams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamExternal> TeamExternals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkflowMaster1> WorkflowMaster1 { get; set; }
    }
}
