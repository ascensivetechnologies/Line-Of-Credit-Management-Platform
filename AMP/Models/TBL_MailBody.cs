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
    
    public partial class TBL_MailBody
    {
        public int id { get; set; }
        public string mail_body { get; set; }
        public Nullable<System.DateTime> mail_sent_date { get; set; }
        public string to_address { get; set; }
        public string cc { get; set; }
        public string bcc { get; set; }
        public string status { get; set; }
        public Nullable<int> RuleTransactionId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string MailSubject { get; set; }
    }
}
