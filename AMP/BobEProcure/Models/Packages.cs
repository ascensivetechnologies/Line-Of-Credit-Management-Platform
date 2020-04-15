using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.BobEProcure.Models
{
    public class Packages
    {
        public int Id { get; set; }
        public string PQId { get; set; }
        public string Ref { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string Approver { get; set; }
        public int? Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? DraftDate { get; set; }
        public string ProjectInfo { get; set; }
        public string LocNumber { get; set; }
        public decimal? PrimaryJvPercentage { get; set; }
        public decimal? SecondaryJvPercentage { get; set; }
        public decimal? LocAmount { get; set; }
        public string TypeOfPackage { get; set; }
        public int? SerialNo { get; set; }
        public int? NoOfPackage { get; set; }
        public decimal? PackageDuration { get; set; }
        public int? LeadMinimumShare { get; set; }
        public int? JvMinShare { get; set; }
        public string PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal? PackageValue { get; set; }
        public int? Index { get; set; }
        public string PackageDisplayId { get; set; }
        public decimal? EstimateValue { get; set; }
        public decimal? AverageRequirement { get; set; }
        public decimal? CashFlowRequirement { get; set; }
        public string PQNo { get; set; }
    }
}