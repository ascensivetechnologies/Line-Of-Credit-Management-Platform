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
    
    public partial class ReviewDeferral1
    {
        public int DeferralID { get; set; }
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        public string StageID { get; set; }
        public string DeferralTimescale { get; set; }
        public string DeferralJustification { get; set; }
        public string ApproverComment { get; set; }
        public string Approver { get; set; }
        public string Approved { get; set; }
        public string Requester { get; set; }
        public Nullable<System.DateTime> PreviousReviewDate { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string Change_Status { get; set; }
    }
}
