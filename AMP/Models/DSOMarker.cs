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
    
    public partial class DSOMarker
    {
        public string ProjectID { get; set; }
        public string GrowGovTradeBaseServ { get; set; }
        public string ClimateChange { get; set; }
        public string ConflictHumaniterian { get; set; }
        public string GlobalPartnerships { get; set; }
        public string MoreEffectiveDoners { get; set; }
        public string HighQualityAid { get; set; }
        public string InternalEfficency { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
    
        public virtual ProjectMaster ProjectMaster { get; set; }
    }
}
