using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.Dashboad2
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ContactTypeId { get; set; }
        public string ContactType { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Landline { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public byte[] _contactImage { get; set; }

        public string Image { get { return this._contactImage != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(this._contactImage)) : null; } }
    }

    public class ProjectContactModel
    {
        public int LinkId { get; set; }
        public int ProjectId { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public int ContactTypeId { get; set; }
        public string ContactType { get; set; }
        public string Organization { get; set; }
    }
}