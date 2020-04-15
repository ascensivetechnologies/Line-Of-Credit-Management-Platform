using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Finacle.Models
{
    public class InterestDueModel
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public DateTime? LastDate { get; set; }
        public DateTime? NextDate { get; set; }
    }
}