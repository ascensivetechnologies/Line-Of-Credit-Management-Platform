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
    
    public partial class TBL_ProjectLogs
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public Nullable<System.DateTime> LoggedOn { get; set; }
    
        public virtual TBL_Projects TBL_Projects { get; set; }
    }
}
