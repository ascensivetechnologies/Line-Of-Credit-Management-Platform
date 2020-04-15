using AMP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class AmendmentsModel
    {
        public int Id { get; set; }

        public DateTime AuditDate { get; set; }
        
        public string AmendedBy { get; set; }

        public string AmendmentDate { get; set; }

        //Loc Terms

        [DisplayName("OM Number")]
        public string OMNumber { get; set; }

        [DisplayName("Terminal date of signing LOC")]
        public string TerminalDate { get; set; }

        [DisplayName("Indian Content Requirement")]
        public float ICR { get; set; }

        [DisplayName("Date of MD Approval")]
        public string MDDate { get; set; }

        [DisplayName("Date of DEA Go Ahead")]
        public string DeaDate { get; set; }

        [DisplayName("Date of MEA Go Ahead")]
        public string MeaDate { get; set; }

        [DisplayName("Date of Offer Letter")]
        public string OfferLetterDate { get; set; }

        [DisplayName("Interest Rate")]
        public float InterestRate { get; set; }

        [DisplayName("Interest Type")]
        public string InterestType { get; set; }

        [DisplayName("Committment Fees")]
        public float CommitmentFee { get; set; }

        [DisplayName("Management Fees")]
        public float ManagementFee { get; set; }

        public string SignedDate { get; set; }

        public string InterestEqualization { get; set; }

        //Interest Equalization
        public string MeaType { get; set; }
        public string DeaType { get; set; }
        public string MeaPercentage { get; set; }
        public string DeaPercentage { get; set; }

        [DisplayName("Tenor")]
        public int? TenureYears { get; set; }

        [DisplayName("Moratorium")]
        public int? Moratorium { get; set; }

        public string SpecialCondition { get; set; }

        [DisplayName("Amount Allocated")]
        public decimal AmountAllocated { get; set; }

        public LocAmmendColors LastAmmendmentComparer { get; set; }
        public AmendmentsModel()
        {
            LastAmmendmentComparer = new LocAmmendColors();
        }
    }
}