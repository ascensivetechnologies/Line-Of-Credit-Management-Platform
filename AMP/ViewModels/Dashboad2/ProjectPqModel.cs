using AMP.Models;
using System;
using System.Collections.Generic;

namespace AMP.ViewModels.Dashboad2
{
    public class ProjectPqModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PQRefNumber { get; set; }
        public string ApplicationStart { get; set; }
        public string ApplicationEnd { get; set; }
        public int ProjectId { get; set; }
        public string AddEndumRefno { get; set; }
        public decimal ProjectCost { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string LocNumber { get; set; }
        public string PqDocPublishedOn { get; set; }
        public string LastSubmissionOn { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string PqStatus { get; set; }
        public int ContractId { get; set; }
        public string ProjectPQSigninDate { get; set; }
        public List<ProjectContractModel> PQContracts { get; set; }
        public List<ApplicantsModel> Applicants { get; set; }
        public List<TBL_Files> Files { get; set; }

        public ProjectPqModel()
        {
            Files = new List<TBL_Files>();
        }
    }
}