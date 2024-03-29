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
    
    public partial class InputSector
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InputSector()
        {
            this.InputSectorCodes = new HashSet<InputSectorCode>();
            this.FundingMechToSectors = new HashSet<FundingMechToSector>();
        }
    
        public string InputSectorCodeID { get; set; }
        public string InputSectorCodeDescription { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InputSectorCode> InputSectorCodes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FundingMechToSector> FundingMechToSectors { get; set; }
    }
}
