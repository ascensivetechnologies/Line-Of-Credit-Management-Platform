

namespace AMP.Models
{
    public class DbStatus
    {
        public int ProcessedId { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }

        public DbStatus()
        {
            Message = "";
        }
    }
}