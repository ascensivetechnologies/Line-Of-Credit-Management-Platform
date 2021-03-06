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
    
    public partial class ComponentMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ComponentMaster()
        {
            this.DeliveryChains = new HashSet<DeliveryChain>();
            this.ImplementingOrganisations = new HashSet<ImplementingOrganisation>();
            this.InputSectorCodes = new HashSet<InputSectorCode>();
        }
    
        public string ComponentID { get; set; }
        public string ComponentDescription { get; set; }
        public string BudgetCentreID { get; set; }
        public string ProjectID { get; set; }
        public string AdminApprover { get; set; }
        public string FundingMechanism { get; set; }
        public string OperationalStatus { get; set; }
        public string BenefittingCountry { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public string Approved { get; set; }
        public string FundingArrangementValue { get; set; }
        public string PartnerOrganisationValue { get; set; }
    
        public virtual ComponentDate ComponentDate { get; set; }
        public virtual BenefitingCountry BenefitingCountry { get; set; }
        public virtual BudgetCentre BudgetCentre { get; set; }
        public virtual ProjectMaster ProjectMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryChain> DeliveryChains { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImplementingOrganisation> ImplementingOrganisations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InputSectorCode> InputSectorCodes { get; set; }
        public virtual Marker Marker { get; set; }
        public virtual Marker Marker1 { get; set; }
    }
}
