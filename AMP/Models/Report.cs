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
    
    public partial class Report
    {
        public string ProjectID { get; set; }
        public int ReportID { get; set; }
        public string ReportType { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> PromptDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<int> QuestNo { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
    
        public virtual ProjectMaster ProjectMaster { get; set; }
    }
}
