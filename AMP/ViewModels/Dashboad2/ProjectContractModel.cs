
namespace AMP.ViewModels.Dashboad2
{
    public class ProjectContractModel
    {
        public int ProjectPqId { get; set; }
        public string PqNo { get; set; }
        public string PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageDisplayId { get; set; }
        public decimal? EstimateValue { get; set; }
        public string TypeOfPackage { get; set; }
        public int ContractId { get; set; }
    }
}