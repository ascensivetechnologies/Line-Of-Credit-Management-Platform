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
    
    public partial class TBL_ContractTerms
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string Milestone { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public string Note { get; set; }
        public Nullable<int> Sequence { get; set; }
        public Nullable<decimal> Amount { get; set; }
    
        public virtual TBL_Contracts TBL_Contracts { get; set; }
    }
}
