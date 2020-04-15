
namespace AMP.ViewModels.Dashboad2
{
    public class ProjectGridModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProjectName { get; set; }
        public string Countries { get; set; }
        public string TeamMembers { get; set; }
        public decimal ProjectValue { get; set; }
        public double ProjectProgress { get; set; }
        public double FinancialProgress { get; set; }
        public string ProjectStatus { get; set; }
        public string CreatedOn { get; set; }
    }
}