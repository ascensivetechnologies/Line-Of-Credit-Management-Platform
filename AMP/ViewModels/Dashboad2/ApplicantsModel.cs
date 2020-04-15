

namespace AMP.ViewModels.Dashboad2
{   
    public class ApplicantsModel
    {
        public int Id { get; set; }
        public string ApplicantNo { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        //public bool PreQualified { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int PqId { get; set; }
        public int ContractId { get; set; }
    }
}