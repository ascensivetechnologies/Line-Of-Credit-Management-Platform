using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.BobEProcure.Models
{
    public class VendorMaster
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string VendorId { get; set; }
        public string Mobile { get; set; }
        public int? LoggedStatus { get; set; }
        public int? UserStatus { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistrationNo { get; set; }
        public string DirectorName { get; set; }
        public string PQCmpAddress { get; set; }
        public string PQCmpEmail { get; set; }
        public string PQCmpMobile { get; set; }
        public string PQCmpWebsite { get; set; }
        public string PQCmpPan { get; set; }
        public string CompanyShortName { get; set; }
        public string cin_no { get; set; }
    }
}