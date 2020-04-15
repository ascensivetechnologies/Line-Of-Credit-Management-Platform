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
    
    public partial class vProjectReview
    {
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        public string ReviewType { get; set; }
        public Nullable<System.DateTime> ReviewDate { get; set; }
        public Nullable<System.DateTime> DeferralDate { get; set; }
        public string RiskScore { get; set; }
        public string Status { get; set; }
        public string Approved { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
        public string OverallScore { get; set; }
        public string Justification { get; set; }
        public string Progress { get; set; }
        public string OnTrackTime { get; set; }
        public string OnTrackCost { get; set; }
        public string OffTrackJustification { get; set; }
        public string FinalOutputScore { get; set; }
        public string OutcomeScore { get; set; }
        public string ProgressToImpact { get; set; }
        public string CompletedToTimescales { get; set; }
        public string CompletedToCost { get; set; }
        public string FailedJustification { get; set; }
    }
}