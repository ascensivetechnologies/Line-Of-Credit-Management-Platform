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
    
    public partial class ComponentDeliveryChain
    {
        public int Identity { get; set; }
        public int ID { get; set; }
        public string ChangeStatus { get; set; }
        public string ComponentID { get; set; }
        public string ChainID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string ParentType { get; set; }
        public Nullable<int> ChildID { get; set; }
        public string ChildType { get; set; }
        public Nullable<int> ParentNodeID { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }
    }
}
