using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.BobEProcure.Models
{
    public class PQStatus
    {
        public int Id { get; set; }
        public string PQSupplierId { get; set; }
        public string PQId { get; set; }
        public string SupplierId { get; set; }
        public int? ParticipationStatus { get; set; }
        public DateTime? ParticipationDate { get; set; }
        public int? ENCStatus { get; set; }
        public string SuppPQSubmId { get; set; }
        public int? Status { get; set; }
        public int? DraftStatus { get; set; }
        public DateTime? SubmisionDate { get; set; }
        public string ApplicantType { get; set; }
        public string SupplierApprDtlId { get; set; }
        public int? PreQualificationStatus { get; set; }
        public string Comments { get; set; }
        public string PQNo { get; set; }
    }
}