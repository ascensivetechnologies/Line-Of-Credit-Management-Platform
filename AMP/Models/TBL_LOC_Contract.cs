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
    
    public partial class TBL_LOC_Contract
    {
        public int Id { get; set; }
        public int LocId { get; set; }
        public int ContractId { get; set; }
        public Nullable<decimal> Value { get; set; }
    
        public virtual TBL_Contracts TBL_Contracts { get; set; }
        public virtual TBL_LOC TBL_LOC { get; set; }
    }
}
