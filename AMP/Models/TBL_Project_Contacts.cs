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
    
    public partial class TBL_Project_Contacts
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ContactId { get; set; }
        public bool IsActive { get; set; }
    
        public virtual TBL_Contacts TBL_Contacts { get; set; }
        public virtual TBL_Projects TBL_Projects { get; set; }
    }
}
