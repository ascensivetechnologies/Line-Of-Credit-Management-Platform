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
    
    public partial class TBL_AuditLogs
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public Nullable<int> RecordId { get; set; }
        public string ActionTaken { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> LoggedOn { get; set; }
    }
}
