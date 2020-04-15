using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string EmployeeNo { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public List<SelectListItem> AllRoles { get
            {
                return new Dashboard2ServiceLayer().GetRoles();
            }
        }

        #region Contact Details
        public byte[] ContactImage { get; set; }
        public int ContactId { get; set; }
        public string Name { get; set; }
        public int? ContactTypeId { get; set; }
        public string ContactType { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Landline { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }

        public List<SelectListItem> ContactTypes {
            get
            {
                return new Dashboard2ServiceLayer().GetAllContactTypes().Select(e => 
                {
                    return new SelectListItem()
                    {
                        Value = e.Id.ToString(),
                        Text = e.Name
                    };
                }).ToList();
            }
        }

        public List<SelectListItem> Countries
        {
            get
            {
                return new Dashboard2ServiceLayer().GetAllCountries().Select(e =>
                {
                    return new SelectListItem()
                    {
                        Value = e.Id.ToString(),
                        Text = e.CountryName
                    };
                }).ToList();
            }
        }

        #endregion

    }
}