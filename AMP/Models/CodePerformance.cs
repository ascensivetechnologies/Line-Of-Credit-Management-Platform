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
    
    public partial class CodePerformance
    {
        public int LogID { get; set; }
        public string MethodName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public Nullable<System.DateTime> From { get; set; }
        public Nullable<System.DateTime> To { get; set; }
        public Nullable<double> Result { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
    }
}
