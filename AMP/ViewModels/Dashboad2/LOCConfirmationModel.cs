using System;

namespace AMP.ViewModels.Dashboad2
{
    public class LOCConfirmationModel
    {
        public int Id { get; set; }
        public int LOCId { get; set; }
        public string Date { get; set; }
        public string ConfirmedBy { get; set; }
        public string Period { get; set; }
    }
}