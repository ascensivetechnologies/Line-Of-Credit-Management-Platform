using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public enum OptionTypes
    {
        [Display(Name = "Risk Classification")]
        Risk = 1,

        [Display(Name = "Loc GuideLines")]
        GuideLines = 2,

        [Display(Name = "Sector")]
        Sector = 3,

        [Display(Name = "Interest Equalization Type")]
        InterestType = 5,

        [Display(Name ="Interest Equalization Approval")]
        InterestApproval = 6,

        [Display(Name = "Contract Type")]
        ContractType = 7,

        [Display(Name = "Content Requirement")]
        ContentRequirement = 8,

        [Display(Name = "Contact Type")]
        ContactType = 9,

        [Display(Name = "Red Flag")]
        RedFlag = 10,

        [Display(Name = "Applicant Status")]
        ApplicantStatus = 11,

        [Display(Name = "PQ Status")]
        PQStatus = 12


    }
}