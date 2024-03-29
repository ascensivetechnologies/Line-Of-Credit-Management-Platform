﻿using AMP.Helpers;
using AMP.Models;
using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AMP.Models.ApiModels;
using System.Configuration;
using AMP.Authentication;
using AMP.ViewModels.DashboardModels;

namespace AMP.Services
{
    public class Dashboard2ServiceLayer
    {
        private AMPEntities ampEntities = new AMPEntities();

        public object ScheduledCompDate { get; private set; }


        #region Regions
        public List<RegionModel> GetAllRegions(string search = "")
        {
            //return ampEntities.Database.SqlQuery<RegionModel>("exec GetRegions").ToList();
            var list = ampEntities.TBL_Regions.ToList().Select(e =>
            {
                return new RegionModel()
                {
                    Name = e.Name,
                    AddedOn = e.AddedOn.ToString("MM/dd/yyyy HH:mm:ss"),
                    AddedBy = e.AddedBy.HasValue ? e.AddedBy.Value.ToString() : "",
                    Id = e.Id,
                    Countries = ampEntities.TBL_Country.ToList().Where(x => x.RegionId == e.Id).Count()
                };
            }).ToList();
            if (!string.IsNullOrWhiteSpace(search))
            {
                list = list.Where(e => e.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            return list;

        }

        public RegionModel GetRegionById(int id)
        {
            return ampEntities.TBL_Regions.ToList().Where(e => e.Id == id).Select(e =>
            {
                return new RegionModel()
                {
                    Name = e.Name,
                    AddedOn = e.AddedOn.ToString("MM/dd/yyyy HH:mm:ss"),
                    AddedBy = e.AddedBy.HasValue ? e.AddedBy.Value.ToString() : "",
                    Id = e.Id
                };
            }).FirstOrDefault();
        }

        public void CreateOrUpdateRegion(RegionModel model)
        {
            var region = ampEntities.TBL_Regions.Where(e => e.Id == model.Id).FirstOrDefault();
            if (region != null)
            {
                region.Name = model.Name;
                region.UpdatedOn = DateTime.Now;
                region.UpdatedBy = model.User.UserId;
            }
            else
            {
                region = new TBL_Regions()
                {
                    AddedOn = DateTime.Now,
                    AddedBy = model.User.UserId,
                    Name = model.Name,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = model.User.UserId
                };
                ampEntities.TBL_Regions.Add(region);
            }
            ampEntities.SaveChanges();
        }

        public void DeleteRegion(int Id)
        {
            var region = ampEntities.TBL_Regions.Where(e => e.Id == Id).FirstOrDefault();
            ampEntities.TBL_Regions.Remove(region);
            ampEntities.SaveChanges();
        }
        #endregion

        #region Countries
        public List<CountryModel> GetAllCountries(string Search = "")
        {
            var list = ampEntities.TBL_Country.ToList().Select(e =>
            {
                var terms = e.TBL_Terms_Country.LastOrDefault();
                return new CountryModel()
                {
                    Id = e.Id,
                    CIF = e.CIF,
                    AddedOn = e.AddedOn.HasValue ? e.AddedOn.Value.ToString("dd/MM/yyyy") : "",
                    AddedBy = e.AddedBy.HasValue ? e.TBL_Users.DisplayName : "",
                    CommitmentFee = terms != null ? float.Parse(terms.CommitmentFee.ToString()) : 0,
                    CountryName = terms != null ? terms.TBL_Country.Name : "",
                    ICR = terms != null ? float.Parse(terms.IndianContribution.ToString()) : 0,
                    InterestRate = terms != null ? float.Parse(terms.InterestRate.ToString()) : 0,
                    LOCClassification = terms != null ? terms.LocGuideLinesClass : "",
                    ManagementFee = terms != null ? float.Parse(terms.ManagementFee.ToString()) : 0,
                    MoratoriumYears = terms != null ? terms.Moratorium : 0,
                    RiskClassification = terms != null ? terms.RiskCategoryClass : "",
                    SpecialConditions = terms != null ? terms.SpecialConditions : "",
                    YearTenure = terms != null ? terms.Tenure : 0,
                    RegionId = terms != null ? terms.TBL_Country.RegionId : 0,
                    RegionName = terms != null ? terms.TBL_Country.TBL_Regions.Name : "",
                    InterestType = terms != null ? terms.InterestType : "",
                    RedFlag = terms != null ? terms.TBL_Country.RedFlag : ""
                    //Regions = ampEntities.TBL_Regions.ToList().Select(x => {
                    //    return new System.Web.Mvc.SelectListItem()
                    //    {
                    //        Text = x.Name,
                    //        Value = x.Id.ToString()
                    //    };
                    //}).ToList(),
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var alphaPager = AlphabetPagerModel.GetAlphabetPager("");
                var isAlpabetSearch = alphaPager.Any(m => m.KeyChar.ToLower() == Search.ToLower());
                if (isAlpabetSearch)
                {
                    list = list.Where(e => e.CountryName.ToLower().StartsWith(Search.ToLower()))
                        .ToList();
                }
                else
                {
                    list = list.Where(e => e.CountryName.ToLower().Contains(Search.ToLower()) ||
                        e.CIF.ToLower().Contains(Search.ToLower()) ||
                        e.RegionName.ToLower().Contains(Search.ToLower())
                    ).ToList();
                }


            }
            return list;
        }

        public CountryModel GetCountryById(int id)
        {
            return ampEntities.TBL_Country.ToList().Where(e => e.Id == id).Select(e =>
            {
                var terms = e.TBL_Terms_Country.LastOrDefault();
                return new CountryModel()
                {
                    Id = e.Id,
                    CIF = e.CIF,
                    CommitmentFee = terms != null ? float.Parse(terms.CommitmentFee.ToString()) : 0,
                    CountryName = e.Name,
                    ICR = terms != null ? float.Parse(terms.IndianContribution.ToString()) : 0,
                    InterestRate = terms != null ? float.Parse(terms.InterestRate.ToString()) : 0,
                    LOCClassification = terms != null ? terms.LocGuideLinesClass : "",
                    ManagementFee = terms != null ? float.Parse(terms.ManagementFee.ToString()) : 0,
                    MoratoriumYears = terms != null ? terms.Moratorium : 0,
                    RiskClassification = terms != null ? terms.RiskCategoryClass : "",
                    SpecialConditions = terms != null ? terms.SpecialConditions : "",
                    YearTenure = terms != null ? terms.Tenure : 0,
                    RegionId = terms != null ? terms.TBL_Country.RegionId : 0,
                    RegionName = terms != null ? terms.TBL_Country.TBL_Regions.Name : "",
                    ApprovalType = terms != null ? terms.ApprovalType : "",
                    Percentage = terms != null ? terms.Percentage : 0,
                    Type = terms != null ? terms.Type : "",
                    InterestType = terms != null ? terms.InterestType : "",
                    RedFlag = terms != null ? terms.TBL_Country.RedFlag : ""
                };
            }).FirstOrDefault();
        }

        public DbStatus CreateOrUpdateCountry(CountryModel model)
        {
            DbStatus status = new DbStatus();
            try
            {
                var record = ampEntities.TBL_Country.Where(e => e.Id == model.Id).FirstOrDefault();
                if (record != null)
                {
                    record.CIF = model.CIF;
                    record.Name = model.CountryName;
                    record.RegionId = model.RegionId;
                    record.UpdatedBy = model.User.UserId;
                    record.UpdatedOn = DateTime.Now;
                    record.RedFlag = model.RedFlag;
                    record.TBL_Terms_Country.Add(new TBL_Terms_Country()
                    {
                        CommitmentFee = model.CommitmentFee,
                        IndianContribution = model.ICR,
                        InterestRate = model.InterestRate,
                        LocGuideLinesClass = model.LOCClassification,
                        ManagementFee = model.ManagementFee,
                        Moratorium = model.MoratoriumYears,
                        RiskCategoryClass = model.RiskClassification,
                        SpecialConditions = model.SpecialConditions,
                        Tenure = model.YearTenure,
                        ApprovalType = model.ApprovalType,
                        Percentage = model.Percentage,
                        Type = model.Type,
                        InterestType = model.InterestType
                    });
                    status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                    status.Status = true;
                }
                else
                {
                    if (ampEntities.TBL_Country.Any(e => e.Name.ToLower() == model.CountryName.ToLower()))
                    {
                        status.Message = StringMapperService.GetValue("DuplicateCountryError");
                        status.Status = false;
                    }
                    else if (ampEntities.TBL_Country.Any(e => e.CIF.ToLower() == model.CIF.ToLower()))
                    {
                        status.Message = StringMapperService.GetValue("DuplicateCIFError");
                        status.Status = false;
                    }
                    else
                    {
                        record = new TBL_Country()
                        {
                            CIF = model.CIF,
                            Name = model.CountryName,
                            RegionId = model.RegionId,
                            AddedBy = model.User.UserId,
                            AddedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = model.User.UserId,
                            RedFlag = model.RedFlag

                        };
                        record.TBL_Terms_Country.Add(new TBL_Terms_Country()
                        {
                            CommitmentFee = model.CommitmentFee,
                            IndianContribution = model.ICR,
                            InterestRate = model.InterestRate,
                            LocGuideLinesClass = model.LOCClassification,
                            ManagementFee = model.ManagementFee,
                            Moratorium = model.MoratoriumYears,
                            RiskCategoryClass = model.RiskClassification,
                            SpecialConditions = model.SpecialConditions,
                            Tenure = model.YearTenure,
                            ApprovalType = model.ApprovalType,
                            Percentage = model.Percentage,
                            Type = model.Type,
                            InterestType = model.InterestType

                        });
                        ampEntities.TBL_Country.Add(record);
                        status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                        status.Status = true;
                    }
                }
                ampEntities.SaveChanges();

            }
            catch (Exception ex)
            {
                status.Message = StringMapperService.GetValue("GeneralUpdateError");
                status.Status = false;
                InsertLog(new LogsModel { CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"), Message = status.Message, FullMessage = ex.Message, ServiceName = "CreateOrUpdateCountry" });
            }
            return status;
        }

        public DbStatus DeleteCountry(int Id)
        {
            DbStatus status = new DbStatus();
            try
            {
                if (ampEntities.TBL_LOC.Any(e => e.CountryId == Id))
                {
                    //error : Country is mapped to an LOC. Please remove before deleting;
                    status.Message = StringMapperService.GetValue("DeleteCountryError");
                    status.Status = false;
                }
                else
                {
                    var record = ampEntities.TBL_Country.Where(e => e.Id == Id).FirstOrDefault();
                    if (record != null)
                    {
                        var record1 = ampEntities.TBL_Terms_Country.Where(e => e.CountryId == Id).FirstOrDefault();
                        if (record1 != null)
                        {
                            ampEntities.TBL_Terms_Country.Remove(record1);
                        }
                        ampEntities.TBL_Country.Remove(record);
                        ampEntities.SaveChanges();
                        status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                        status.Status = true;
                    }
                    else
                    {
                        status.Message = StringMapperService.GetValue("DataNotFoundError");
                        status.Status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = StringMapperService.GetValue("GeneralUpdateError");
                status.Status = false;
                InsertLog(new LogsModel { CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"), Message = status.Message, FullMessage = ex.Message, ServiceName = "DeleteCountry" });
            }
            return status;
        }

        #endregion

        #region Terms
        public List<TermsModel> GetAllTerms(string Search = "")
        {
            var list = ampEntities.TBL_Terms.ToList().Select(e =>
            {
                return new TermsModel()
                {
                    Id = e.Id,
                    ApprovalType = e.ApprovalType,
                    CommitmentFee = e.CommitmentFee,
                    ICR = e.IndianContribution,
                    InterestRate = e.InterestRate,
                    ManagementFee = e.ManagementFee,
                    MoratoriumYears = e.Moratorium,
                    Percentage = e.Percentage,
                    SpecialConditions = e.SpecialConditions,
                    Type = e.Type,
                    YearTenure = e.Tenure,
                    LOCClassification = e.LOCClassification,
                    RiskClassification = e.RiskClassification,
                    InterestType = e.InterestType,

                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                list = list.Where(e => e.LOCClassification.ToLower().Contains(Search.ToLower())).ToList();
            }
            return list;
        }

        public TermsModel GetTermsById(int id)
        {
            return ampEntities.TBL_Terms.ToList().Where(e => e.Id == id).Select(e =>
            {
                return new TermsModel()
                {
                    Id = e.Id,
                    ApprovalType = e.ApprovalType,
                    CommitmentFee = e.CommitmentFee,
                    ICR = e.IndianContribution,
                    InterestRate = e.InterestRate,
                    ManagementFee = e.ManagementFee,
                    MoratoriumYears = e.Moratorium,
                    Percentage = e.Percentage,
                    SpecialConditions = e.SpecialConditions,
                    Type = e.Type,
                    YearTenure = e.Tenure,
                    LOCClassification = e.LOCClassification,
                    RiskClassification = e.RiskClassification,
                    InterestType = e.InterestType,
                };
            }).FirstOrDefault();
        }

        public void CreateOrUpdateTerms(TermsModel model)
        {
            var record = ampEntities.TBL_Terms.Where(e => e.Id == model.Id).FirstOrDefault();
            if (record != null)
            {
                record.ApprovalType = model.ApprovalType;
                record.CommitmentFee = model.CommitmentFee;
                record.IndianContribution = model.ICR;
                record.InterestRate = model.InterestRate;
                record.ManagementFee = model.ManagementFee;
                record.Moratorium = model.MoratoriumYears;
                record.Percentage = model.Percentage;
                record.SpecialConditions = model.SpecialConditions;
                record.Tenure = model.YearTenure;
                record.Type = model.Type;
                record.RiskClassification = model.RiskClassification;
                record.LOCClassification = model.LOCClassification;
                record.InterestType = model.InterestType;
                record.UpdatedBy = model.User.UserId;
                record.UpdatedOn = DateTime.Now;

            }
            else
            {
                record = new TBL_Terms()
                {
                    ApprovalType = model.ApprovalType,
                    CommitmentFee = model.CommitmentFee,
                    IndianContribution = model.ICR,
                    InterestRate = model.InterestRate,
                    ManagementFee = model.ManagementFee,
                    Moratorium = model.MoratoriumYears,
                    Percentage = model.Percentage,
                    SpecialConditions = model.SpecialConditions,
                    Tenure = model.YearTenure,
                    Type = model.Type,
                    LOCClassification = model.LOCClassification,
                    RiskClassification = model.RiskClassification,
                    InterestType = model.InterestType,
                    AddedBy = model.User.UserId,
                    AddedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = model.User.UserId
                };
                ampEntities.TBL_Terms.Add(record);
            }
            ampEntities.SaveChanges();

        }

        public void DeleteTerms(int Id)
        {
            var record = ampEntities.TBL_Terms.Where(e => e.Id == Id).FirstOrDefault();
            if (record != null)
            {
                ampEntities.TBL_Terms.Remove(record);
                ampEntities.SaveChanges();
            }
        }
        #endregion

        #region Loc
        public IEnumerable<LOCModel> GetAllLocs(string Search = "")
        {
            IEnumerable<LOCModel> result = GetLocQueryForGrid(Search);
            //List<TBL_LOC> _list = new List<TBL_LOC>();
            //List<LOCModel> list = new List<LOCModel>();
            //if (!string.IsNullOrWhiteSpace(Search))
            //    _list = ampEntities.TBL_LOC.Where(e => e.Name.ToLower().Contains(Search.ToLower())).ToList();
            //else
            //    _list = ampEntities.TBL_LOC.ToList();
            //foreach (var item in _list)
            //{
            //    list.Add(ConvertLocEntityToModel(item));
            //}
            return result;
        }

        private IQueryable<LOCModel> GetLocQueryForGrid(string Search)
        {
            var query = ampEntities.TBL_LOC.Select(p => new
            {
                p.Id,
                p.LocNumber,
                p.SigningDate,
                p.Name,
                p.CountryId,
                p.TotalAmount,
                CountryName = p.TBL_Country.Name,
                p.TBL_Country.CIF,
                AssociatedProjects = p.TBL_LOC_Project.Count(),
                SanctionAmount = ampEntities.Finacle_LocDetails.Where(fl => fl.LimitPrefix.Equals(p.LocNumber.Replace("-", "")))
                    .Select(fl => fl.SanctionAmount.HasValue ? fl.SanctionAmount : 0).FirstOrDefault(),
                DisbAmt = ampEntities.Finacle_Disbursement.Where(e => e.FORACID.Equals(p.LocAccountNo)).Sum(a => a.DisbAmount)
            })
                .Select(p2 => new
                {
                    p2.Id,
                    p2.LocNumber,
                    p2.Name,
                    p2.SigningDate,
                    p2.CountryId,
                    p2.CountryName,
                    p2.TotalAmount,
                    p2.CIF,
                    p2.AssociatedProjects,
                    Utilization = p2.SanctionAmount.HasValue ? (p2.DisbAmt / p2.SanctionAmount.Value * 100) : 0,
                })
                .Select(p3 => new LOCModel
                {
                    Id = p3.Id,
                    LOCNumber = p3.LocNumber,
                    LOCName = p3.Name,
                    _SignedDate = p3.SigningDate,
                    CountryId = p3.CountryId,
                    CountryName = p3.CountryName,
                    AmountAllocated = p3.TotalAmount.HasValue ? p3.TotalAmount.Value.ToString() : "",
                    CIF = p3.CIF,
                    AssociatedProjects = p3.AssociatedProjects,
                    Utilization = p3.Utilization.HasValue ? p3.Utilization.Value : 0,
                    status = p3.Utilization < 100 ? true : false
                });
            if (!string.IsNullOrEmpty(Search))
                query = query.Where(m => m.LOCName.ToLower().Contains(Search.ToLower()));
            return query;
        }

        public IEnumerable<LOCModel> GetMyLocs(string Search = "")
        {

            var user = UserConversion.Convertuser().UserId;
            if (user != 0)
            {
                var entityIds = ampEntities.TBl_TeamMapping.Where(t => t.UserId == user
                && t.EntityName.ToLower().Equals("tbl_loc")).Select(t => t.EntityId);
                var query = GetLocQueryForGrid(Search).Where(m => entityIds.Contains(m.Id));
                //var records = ampEntities.TBl_TeamMapping.ToList().Where(e => e.UserId == user && (string.Compare("TBL_Loc", e.EntityName, StringComparison.OrdinalIgnoreCase) == 0)).Select(e =>
                //{
                //    return GetLocById(e.EntityId);
                //}).ToList();

                //if (!string.IsNullOrWhiteSpace(Search))
                //{
                //    records = records.Where(e => e.LOCName.ToLower().Contains(Search.ToLower())).ToList();
                //}
                return query;
            }
            return new List<LOCModel>();
        }

        public LOCModel GetLocById(int id)
        {
            var loc = ampEntities.TBL_LOC.Where(e => e.Id == id).FirstOrDefault();
            LOCModel locDetail = new LOCModel();
            if (loc != null)
            {
                locDetail = ConvertLocEntityToModel(loc);
                locDetail.Files = GetAllFiles(Utilities.Constants.LOC, locDetail.Id);
            }
            return locDetail;
        }

        public DbStatus CreateOrUpdateLoc(LOCModel model)
        {
            string sql = "";
            var parameters = GetSqlParameterForLOC(model);
            DbStatus status = new DbStatus();
            sql = "Execute CreateUpdate_Loc @signingDate, @terminalDate, @meaDate, @mdDate, @offerDate, @omNumber, @amountAllocated, "
                    + "@interest, @commitmentFee, @managementFee, @equalization, @mea_percentage, @dea_percentage, @approvalType, @tenure, @moratorium, "
                    + "@indianContribution, @purpose, @locId, @countryId, @approvalBy, @approvalDate, @ammendmentNumber, "
                    + "@goiDeedDate, @effectiveDate, @aggrementAmmendDate, @name, @locNumber, @locAccountNumber, "
                    + "@mea_type, @dea_type, @classification, @deadate, @specialcondition, @user, @vnote, @anote, @interesttype ";
            status = ExecuteSqlCommand(sql, parameters);
            if (status.Status && status.ProcessedId > 0)
            {
                model.Id = status.ProcessedId;
                AddTeamMember(UserConversion.Convertuser().UserId, status.ProcessedId, "TBL_Loc");
            }

            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.Id,
                    EntityName = "TBL_LOC",
                    Message = "Updated the Loc.",
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }

            return status;
        }

        private static AmendmentsModel ConvertAmendmentEntityToModel(TBL_LOC_Amendments amd)
        {
            return new AmendmentsModel
            {
                Id = amd.Id,
                AuditDate = amd.AuditDate,
                AmendmentDate = amd.AgreementAmendmentDate.HasValue ? amd.AgreementAmendmentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA",
                AmendedBy = string.IsNullOrEmpty(amd.AmendedBy) ? "NA" : amd.AmendedBy,
                TerminalDate = amd.TerminalDate.HasValue ? amd.TerminalDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA",
                ICR = amd.IndianContribution.HasValue ? float.Parse(amd.IndianContribution.ToString()) : 0,
                MDDate = amd.MdAppDate.HasValue ? amd.MdAppDate.Value.ToString("dd/MM/yyyy") : "NA",
                DeaDate = amd.DEAAppDate.HasValue ? amd.DEAAppDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA",
                MeaDate = amd.MEAAppDate.HasValue ? amd.MEAAppDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA",
                OfferLetterDate = amd.OfferLetterDate.HasValue ? amd.OfferLetterDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA",
                InterestRate = float.Parse((amd.InterestRate.HasValue ? amd.InterestRate.Value : 0).ToString()),
                CommitmentFee = float.Parse((amd.CommitmentFee.HasValue ? amd.CommitmentFee.Value : 0).ToString()),
                ManagementFee = float.Parse((amd.ManagementFee.HasValue ? amd.ManagementFee.Value : 0).ToString()),
                SignedDate = amd.SigningDate.HasValue ? amd.SigningDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA",
                InterestEqualization = string.IsNullOrEmpty(amd.InterestEqualization) ? "NA" : amd.InterestEqualization,
                MeaType = amd.MEA_Type,
                DeaType = amd.DEA_Type,
                MeaPercentage = (amd.MEA_Percentage.HasValue ? amd.MEA_Percentage.Value : 0).ToString("0.00"),
                DeaPercentage = (amd.DEA_Percentage.HasValue ? amd.DEA_Percentage.Value : 0).ToString("0.00"),
                TenureYears = amd.Tenure,
                Moratorium = amd.Moratorium,
                SpecialCondition = amd.SpecialCondition,
                InterestType = string.IsNullOrEmpty(amd.InterestType) ? "NA" : amd.InterestType,
                OMNumber = string.IsNullOrEmpty(amd.OmNumber) ? "NA" : amd.OmNumber,
                AmountAllocated = amd.AmountAllocated.HasValue ? amd.AmountAllocated.Value : 0
            };
        }

        private static List<SqlParameter> GetSqlParameterForLOC(LOCModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            var date = DateTimeHelper.ToDateObject(model.SignedDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@signingDate", date));
            else
                param.Add(new SqlParameter("@signingDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.TerminalDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@terminalDate", date));
            else
                param.Add(new SqlParameter("@terminalDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.MeaDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@meaDate", date));
            else
                param.Add(new SqlParameter("@meaDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.MDDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@mdDate", date));
            else
                param.Add(new SqlParameter("@mdDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.OfferLetterDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@offerDate", Convert.ToDateTime(date)));
            else
                param.Add(new SqlParameter("@offerDate", DBNull.Value));

            if (string.IsNullOrEmpty(model.OMNumber))
                param.Add(new SqlParameter("@omNumber", DBNull.Value));
            else
                param.Add(new SqlParameter("@omNumber", model.OMNumber));



            if (string.IsNullOrEmpty(model.AmountAllocated))
                param.Add(new SqlParameter("@amountAllocated", DBNull.Value));
            else
                param.Add(new SqlParameter("@amountAllocated", model.AmountAllocated));


            param.Add(new SqlParameter("@interest", model.InterestRate));
            param.Add(new SqlParameter("@commitmentFee", model.CommitmentFee));
            param.Add(new SqlParameter("@managementFee", model.ManagementFee));

            if (string.IsNullOrEmpty(model.InterestEqualization))
                param.Add(new SqlParameter("@equalization", DBNull.Value));
            else
                param.Add(new SqlParameter("@equalization", model.InterestEqualization));

            param.Add(new SqlParameter("@mea_percentage", model.MeaPercentage));
            param.Add(new SqlParameter("@dea_percentage", model.DeaPercentage));

            if (string.IsNullOrEmpty(model.ApprovalType))
                param.Add(new SqlParameter("@approvalType", DBNull.Value));
            else
                param.Add(new SqlParameter("@approvalType", model.ApprovalType));

            if (model.TenureYears.HasValue)
                param.Add(new SqlParameter("@tenure", model.TenureYears));
            else
                param.Add(new SqlParameter("@tenure", DBNull.Value));

            if (model.Moratorium.HasValue)
                param.Add(new SqlParameter("@moratorium", model.Moratorium));
            else
                param.Add(new SqlParameter("@moratorium", DBNull.Value));

            param.Add(new SqlParameter("@indianContribution", model.ICR));

            if (string.IsNullOrEmpty(model.LocPurpose))
                param.Add(new SqlParameter("@purpose", DBNull.Value));
            else
                param.Add(new SqlParameter("@purpose", model.LocPurpose));

            param.Add(new SqlParameter("@locId", model.Id));

            if (model.CountryId.HasValue)
                param.Add(new SqlParameter("@countryId", model.CountryId));
            else
                param.Add(new SqlParameter("@countryId", DBNull.Value));

            if (model.ApprovedBy.HasValue)
                param.Add(new SqlParameter("@approvalBy", model.ApprovedBy));
            else
                param.Add(new SqlParameter("@approvalBy", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.ApprovedOn);
            if (date.HasValue)
                param.Add(new SqlParameter("@approvalDate", date));
            else
                param.Add(new SqlParameter("@approvalDate", DBNull.Value));

            if (model.AmmendmentNumber.HasValue)
                param.Add(new SqlParameter("@ammendmentNumber", model.ApprovedBy));
            else
                param.Add(new SqlParameter("@ammendmentNumber", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.GOIDeedDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@goiDeedDate", date));
            else
                param.Add(new SqlParameter("@goiDeedDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.EffectiveDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@effectiveDate", date));
            else
                param.Add(new SqlParameter("@effectiveDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(model.AgreementAmmendDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@aggrementAmmendDate", date));
            else
                param.Add(new SqlParameter("@aggrementAmmendDate", DBNull.Value));

            if (string.IsNullOrEmpty(model.LOCName))
                param.Add(new SqlParameter("@name", DBNull.Value));
            else
                param.Add(new SqlParameter("@name", model.LOCName));

            if (string.IsNullOrEmpty(model.LOCNumber))
                param.Add(new SqlParameter("@locNumber", DBNull.Value));
            else
                param.Add(new SqlParameter("@locNumber", model.LOCNumber));

            if (string.IsNullOrEmpty(model.LOCAccountNumber))
                param.Add(new SqlParameter("@locAccountNumber", DBNull.Value));
            else
                param.Add(new SqlParameter("@locAccountNumber", model.LOCAccountNumber));

            if (string.IsNullOrEmpty(model.MeaType))
                param.Add(new SqlParameter("@mea_type", DBNull.Value));
            else
                param.Add(new SqlParameter("@mea_type", model.MeaType));

            if (string.IsNullOrEmpty(model.DeaType))
                param.Add(new SqlParameter("@dea_type", DBNull.Value));
            else
                param.Add(new SqlParameter("@dea_type", model.DeaType));

            if (string.IsNullOrEmpty(model.Classification))
                param.Add(new SqlParameter("@classification", DBNull.Value));
            else
                param.Add(new SqlParameter("@classification", model.Classification));

            date = DateTimeHelper.ToDateObject(model.DeaDate);
            if (!date.HasValue)
                param.Add(new SqlParameter("@deadate", DBNull.Value));
            else
                param.Add(new SqlParameter("@deadate", date));

            if (string.IsNullOrEmpty(model.SpecialCondition))
                param.Add(new SqlParameter("@specialcondition", DBNull.Value));
            else
                param.Add(new SqlParameter("@specialcondition", model.SpecialCondition));

            if (string.IsNullOrWhiteSpace(model.VerificationNote))
                param.Add(new SqlParameter("@vnote", DBNull.Value));
            else
                param.Add(new SqlParameter("@vnote", model.VerificationNote));

            //if (string.IsNullOrEmpty())
            //param.Add(new SqlParameter("@user", DBNull.Value));
            //else

            param.Add(new SqlParameter("@user", UserConversion.Convertuser().Username));

            if (string.IsNullOrWhiteSpace(model.AmendmentNote))
                param.Add(new SqlParameter("@anote", DBNull.Value));
            else
                param.Add(new SqlParameter("@anote", model.AmendmentNote));

            param.Add(new SqlParameter("@interesttype", model.InterestType));
            return param;
        }

        public void LinkProjectToLOC(LOCProjectModel project)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                TBL_LOC_Project _project = new TBL_LOC_Project();
                _project.Allocation = project.Allocation;
                _project.LocId = project.LocId;
                _project.ProjectId = project.ProjectId;

                entity.TBL_LOC_Project.Add(_project);

                var db_project = entity.TBL_Projects.FirstOrDefault(e => e.Id == project.ProjectId);

                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = project.LocId,
                    EntityName = "TBL_LOC",
                    Message = string.Format("Added Project {0}", db_project != null ? db_project.Name : ""),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });

                entity.SaveChanges();
            }

        }

        public void UpdateLinkedLOCAmount(LOCProjectModel project)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                TBL_LOC_Project _project = new TBL_LOC_Project();
                var linkedEntity = entity.TBL_LOC_Project.Where(m => m.Id == project.Id).FirstOrDefault();
                if (linkedEntity != null)
                {
                    linkedEntity.Allocation = project.Allocation;
                    var locName = linkedEntity.TBL_LOC.Name;
                    //ACTIVITY
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = project.LocId,
                        EntityName = "TBL_LOC",
                        Message = string.Format("Updated {0} LOC Allocation Amount to {1}", locName, project.Allocation),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });

                    entity.SaveChanges();
                }

            }
        }

        public void RemoveLinkedLocOfProject(int linkId)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var linkEntity = entity.TBL_LOC_Project.Where(m => m.Id == linkId).FirstOrDefault();
                if (linkEntity != null)
                {
                    entity.TBL_LOC_Project.Remove(linkEntity);
                    entity.SaveChanges();
                }
            }
        }

        public void AddBalanceConfirmationToLOC(LOCConfirmationModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                entity.TBL_LocBalance.Add(new TBL_LocBalance()
                {
                    ConfirmedBy = model.ConfirmedBy,
                    Date = DateTimeHelper.ToDateObject(model.Date),
                    LocId = model.LOCId,
                    Period = model.Period,

                });

                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.LOCId,
                    EntityName = "TBL_LOC",
                    Message = string.Format("Added Balance Confimation of Period {0}, Confirmed By {1} dated {2}", model.Period, model.ConfirmedBy, model.Date),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
        }

        public void DeleteLoc(int Id)
        {

        }

        private static LOCModel ConvertLocEntityToModel(TBL_LOC loc)
        {
            LOCModel l = new LOCModel();

            l.AgreementAmmendDate = loc.AgreementAmendmentDate.HasValue ?
                loc.AgreementAmendmentDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.AmmendmentNumber = loc.AmendmentNumber;
            l.DeaDate = loc.DeaDate.HasValue ? loc.DeaDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.ApprovalType = loc.ApprovalType;
            l.ApprovedBy = loc.ApprovalBy;
            l.ApprovedOn = loc.ApprovalDate.HasValue
                ? loc.ApprovalDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.Classification = string.IsNullOrEmpty(loc.Classification) ? "NA" : loc.Classification;
            l.CommitmentFee = float.Parse(loc.CommitmentFee.ToString());
            l.CountryName = string.Format("{0},{1}", loc.TBL_Country.Name, loc.TBL_Country.TBL_Regions.Name);
            l.CountryId = loc.CountryId;
            l.EffectiveDate = loc.EffectiveDate.HasValue ?
                loc.EffectiveDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.GOIDeedDate = loc.GOIDeedDate.HasValue ?
                loc.GOIDeedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.ICR = loc.IndianContribution.HasValue ? float.Parse(loc.IndianContribution.ToString()) : 0;
            l.InterestRate = float.Parse(loc.InterestRate.ToString());
            l.Id = loc.Id;
            l.InterestEqualization = string.IsNullOrEmpty(loc.InterestEqualization) ? "NA" : loc.InterestEqualization;
            l.LOCAccountNumber = string.IsNullOrEmpty(loc.LocAccountNo) ? "NA" : loc.LocAccountNo;
            l.LOCName = loc.Name;
            l.LOCNumber = loc.LocNumber;
            l.LocPurpose = string.IsNullOrEmpty(loc.Purpose) ? "NA" : loc.Purpose;
            l.ManagementFee = float.Parse(loc.ManagementFee.ToString());
            l.MDDate = loc.MdAppDate.HasValue ? loc.MdAppDate.Value.ToString("dd/MM/yyyy") : "NA";
            l.Moratorium = loc.Moratorium;
            l.OfferLetterDate = loc.OfferLetterDate.HasValue ? loc.OfferLetterDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.OMNumber = string.IsNullOrEmpty(loc.OmNumber) ? "NA" : loc.OmNumber;
            l.MeaPercentage = float.Parse((loc.MEA_Percentage.HasValue ? loc.MEA_Percentage.Value : 0).ToString("0.00"));
            l.DeaPercentage = float.Parse((loc.DEA_Percentage.HasValue ? loc.DEA_Percentage.Value : 0).ToString("0.00"));
            l.SignedDate = loc.SigningDate.HasValue ? loc.SigningDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.TenureYears = loc.Tenure;
            l.TerminalDate = loc.TerminalDate.HasValue ? loc.TerminalDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            l.MeaType = string.IsNullOrEmpty(loc.MEA_Type) ? "" : loc.MEA_Type;
            l.DeaType = string.IsNullOrEmpty(loc.DEA_Type) ? "" : loc.DEA_Type;
            l.MeaDate = loc.MEAAppDate.HasValue ? loc.MEAAppDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "NA";
            try { l.AssociatedProjects = loc.TBL_LOC_Project.Count(); }
            catch { l.AssociatedProjects = 0; }
            l.VerificationNote = string.IsNullOrEmpty(loc.VerificationNote) ? "NA" : loc.VerificationNote;
            l.InterestType = string.IsNullOrEmpty(loc.InterestType) ? "" : loc.InterestType;
            l.SpecialCondition = string.IsNullOrEmpty(loc.SpecialCondition) ? "" : loc.SpecialCondition;
            l.Balanaces = loc.TBL_LocBalance.ToList().Select(e =>
            {
                return new BalanceModel()
                {
                    ConfirmedBy = e.ConfirmedBy,
                    Date = e.Date.HasValue ? e.Date.Value.ToString("dd/MM/yyyy") : "NA",
                    Period = e.Period,
                    Id = e.Id
                };
            }).ToList();

            l.LocProjectGridData = loc.TBL_LOC_Project.Select(e =>
            {
                //float ptot = e.TBL_Projects.ProjectStart.HasValue ? e.TBL_Projects.ProjectEnd.HasValue ? ((TimeSpan)(e.TBL_Projects.ProjectEnd - e.TBL_Projects.ProjectStart)).Days : 0 : 0;
                //float pnow = e.TBL_Projects.ProjectStart.HasValue ? ((TimeSpan)(DateTime.Now - e.TBL_Projects.ProjectStart)).Days : 0;
                //float prog = (ptot > 0 && pnow > 0) ? (pnow > ptot) ? 100f : (pnow / ptot * 100f) : 0;
                //if (ptot > 0 && pnow > 0)
                //{
                //    if (pnow > ptot)
                //        prog = 100;
                //    else
                //        prog = pnow / ptot * 100;
                //}


                return new LOCModel.LocProjectGridModal()
                {
                    Code = e.TBL_Projects.Code,
                    Name = e.TBL_Projects.Name,
                    Allocation = e.Allocation.HasValue ? e.Allocation.Value : 0,
                    FinancialProgress = float.Parse((e.TBL_Projects.FinancialProgress.HasValue ? e.TBL_Projects.FinancialProgress : 0).ToString()),
                    PhysicalProgress = float.Parse((e.TBL_Projects.Progress.HasValue ? e.TBL_Projects.Progress : 0).ToString()),//((ptot > 0) && (pnow > 0)) ? ((pnow > ptot) ? 100f : ((pnow / ptot) * 100f)) : 0,
                    ProjectValue = e.TBL_Projects.ProjectValue,
                    ProjectId = e.TBL_Projects.Id.ToString()
                };
            }).ToList();

            l.CIF = loc.TBL_Country.CIF;
            //l.Utilization = 66;
            //l.status = false;

            l.Amendments = loc.TBL_LOC_Amendments.Select(a => ConvertAmendmentEntityToModel(a)).OrderByDescending(e => e.Id).ToList();
            SetAmmendmentsColorCode(loc, l);
            l.Region = new RegionModel()
            {
                AddedBy = loc.TBL_Country.TBL_Regions.AddedBy.HasValue ? loc.TBL_Country.TBL_Regions.AddedBy.Value.ToString() : "NA",
                AddedOn = loc.TBL_Country.TBL_Regions.AddedOn.ToString("dd/MM/yyyy"),
                Name = loc.TBL_Country.TBL_Regions.Name,
                Id = loc.TBL_Country.RegionId
            };

            #region Finacle
            using (AMPEntities entity = new AMPEntities())
            {
                var record = entity.Finacle_Disbursement.Where(e => e.FORACID.Equals(l.LOCAccountNumber)).OrderBy(e => e.DisDate).FirstOrDefault();
                if (record != null)
                {
                    l.DisbursementUnderLoc = record.DisDate.HasValue ? record.DisDate.Value.ToString("dd/MM/yyyy") : "";

                    var interestDue = entity.Finacle_InterestDue.ToList().Where(e => e.AccountId.Equals(record.FORACID)).FirstOrDefault();
                    if (interestDue != null)
                    {
                        l.InterestDueDate = string.Join(", ",
                                                                interestDue.LastDate.HasValue ? interestDue.LastDate.Value.ToString("dd MMMM") : "",
                                                                interestDue.NextDate.HasValue ? interestDue.NextDate.Value.ToString("dd MMMM") : ""
                            );
                    }

                    var repayments = entity.Finacle_PrincipalDue.ToList().Where(e => e.AccountId.Equals(record.FORACID) && e.DemandType.Equals("PRDEM") && e.DueDate > DateTime.Now).OrderBy(e => e.DueDate).Take(2);
                    if (repayments.Any())
                    {
                        var repaymentDates = repayments.Select(x => x.DueDate.HasValue ? x.DueDate.Value.ToString("dd MMMM") : "");
                        l.RepaymentDueDate = string.Join(", ", repaymentDates);
                    }
                }



                //var repayment = entity.Finacle_RepaymentSchedule.Where(e => e.LimitPrefix.Equals(l.LOCNumber)).OrderBy(e => e.FlowStart).ToList();
                //if (repayment.Any())
                //{
                //    var re1 = repayment.Where(e => e.FlowId.Equals("PRDEM")).FirstOrDefault();
                //    l.PrincipalRepaymentDate = re1 != null ? (re1.FlowStart.HasValue ? re1.FlowStart.Value.ToString(("dd/MM/yyyy")) : "") : "";

                //    var re = repayment.Where(e => e.FlowId.Equals("PRDEM")).LastOrDefault();
                //    l.PrincipalRepaymentDateEnd = re != null ? (re.FlowStart.HasValue ? re.FlowStart.Value.ToString(("dd/MM/yyyy")) : "") : "";
                //}

                var repayment = entity.Finacle_PrincipalDue.Where(e => e.AccountId.Equals(l.LOCAccountNumber)).OrderBy(e => e.DueDate).ToList();
                if (repayment.Any())
                {
                    var re1 = repayment.Where(e => e.DemandType.Equals("PRDEM")).FirstOrDefault();
                    l.PrincipalRepaymentDate = re1 != null ? (re1.DueDate.HasValue ? re1.DueDate.Value.ToString(("dd/MM/yyyy")) : "") : "";

                    var re = repayment.Where(e => e.DemandType.Equals("PRDEM")).LastOrDefault();
                    l.PrincipalRepaymentDateEnd = re != null ? (re.DueDate.HasValue ? re.DueDate.Value.ToString(("dd/MM/yyyy")) : "") : "";
                }

                var fLoc = entity.Finacle_LocDetails.Where(e => e.LimitPrefix.Equals(l.LOCNumber.Replace("-",""))).FirstOrDefault();
                var disbamnt = entity.Finacle_Disbursement.Where(e => e.FORACID.Equals(l.LOCAccountNumber)).Sum(a => a.DisbAmount);
                if (fLoc != null)
                {
                    //  l.AmountAllocated = fLoc.SanctionAmount.HasValue ? string.Format("{0:}", Math.Round(fLoc.SanctionAmount.Value)) : "---";
                    l.SanctionAmount = fLoc.SanctionAmount.HasValue ? string.Format("{0:}", Math.Round(fLoc.SanctionAmount.Value)) : "---";
                    l.Utilization = Convert.ToInt32(fLoc.SanctionAmount.HasValue ? disbamnt / fLoc.SanctionAmount.Value * 100 : 0);
                }
                else
                {
                    l.AmountAllocated = loc.TotalAmount.HasValue ? loc.TotalAmount.Value.ToString("0.00") : "";
                    l.Utilization = 0;
                }

                if (fLoc != null)
                {
                    l.AmountAllocated = loc.TotalAmount.HasValue ? loc.TotalAmount.Value.ToString("0.00") : "";
                }

                l.AmountDisbursed = disbamnt.HasValue ? disbamnt.Value.ToString("0.00") : "";

                l.status = l.Utilization < 100 ? true : false;

                var financials = entity.Finacle_LocFinancials.Where(e => e.FORACID == l.LOCAccountNumber).ToList().FirstOrDefault();
                if (financials != null)
                {
                    l.LoanOutstanding = financials.LoanOutstanding.HasValue ? financials.LoanOutstanding.Value.ToString("0.00") : "";
                    l.TotalDisbursed = financials.TotalDisbursed.HasValue ? financials.TotalDisbursed.Value.ToString("0.00") : "";
                    l.PrincipalDemand = financials.PrincipalDemand.HasValue ? financials.PrincipalDemand.Value.ToString("0.00") : "";
                    l.PrincipalCollection = financials.PrincipalCollection.HasValue ? financials.PrincipalCollection.Value.ToString("0.00") : "";
                    l.PrincipalOverdue = financials.PrincipalOverdue.HasValue ? financials.PrincipalOverdue.Value.ToString("0.00") : "";
                    l.InterestDemand = financials.InterestDemand.HasValue ? financials.InterestDemand.Value.ToString("0.00") : "";
                    l.InterestCollection = financials.InterestCollection.HasValue ? financials.InterestCollection.Value.ToString("0.00") : "";
                    l.InterestOverdue = financials.InterestOverdue.HasValue ? financials.InterestOverdue.Value.ToString("0.00") : "";
                }
            }

            #endregion

            return l;
        }

        private static void SetAmmendmentsColorCode(TBL_LOC loc, LOCModel l)
        {
            var ammendsColor = GetLocAmmendsColor(loc.Id);
            if (ammendsColor.Any())
            {
                foreach (var loc_amd in l.Amendments)
                {
                    var colorcode = ammendsColor.Where(m => m.CompId == loc_amd.Id).FirstOrDefault();
                    if (colorcode != null)
                    {
                        loc_amd.LastAmmendmentComparer = colorcode;
                    }
                    else
                    {
                        loc_amd.LastAmmendmentComparer = new LocAmmendColors();
                    }
                }
            }
        }

        public static List<LocAmmendColors> GetLocAmmendsColor(int locId)
        {
            string sql = "Execute Get_Color_ChangesFor_Loc " + locId;
            AMPEntities ampEntities = new AMPEntities();
            var list = ampEntities.Database.SqlQuery<LocAmmendColors>(sql)
                .ToList();
            return list;
        }
        #endregion

        #region Projects

        public ProjectModel GetProjectById(int id)
        {
            var project = ampEntities.TBL_Projects.ToList().Where(e => e.Id == id).Select(x =>
            {
                return new ProjectModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Code = x.Code,
                    BaselineData = x.BaselineData,
                    DPRDate = x.DPRDate,
                    ProjectEndDate = x.ProjectEnd,
                    ProjectStartDate = x.ProjectStart,
                    ProjectValue = x.ProjectValue,
                    ProjectProgress = x.Progress.HasValue ? Convert.ToDecimal(x.Progress) : 0,
                    Stage = x.Stage.HasValue ? x.Stage.Value : 0,
                    Status = x.Status.HasValue ? x.Status.Value : 0,
                    SubSector = x.SubSector.HasValue ? x.SubSector.Value : 0,
                    Sector = x.Sector,
                    SubSectorName = x.SubSectorName,
                    Address = x.Address,
                    Note = x.Note
                };

            }).FirstOrDefault();
            project.StartDateTxt = DateTimeHelper.ToDateString(project.ProjectStartDate);
            project.EndDateTxt = DateTimeHelper.ToDateString(project.ProjectEndDate);
            return project;
        }

        public ProjectDetailModel GetProjectDetailModel(int id)
        {
            ProjectDetailModel model = new ProjectDetailModel();
            var entity = ampEntities.TBL_Projects.Where(m => m.Id == id).FirstOrDefault();
            if (entity != null)
            {
                model.Id = entity.Id;
                model.Code = entity.Code;
                model.Note = entity.Note;
                model.Description = entity.Description;
                if (entity.ProjectStart.HasValue && entity.ProjectEnd.HasValue)
                {
                    var totalDays = (entity.ProjectEnd.Value - entity.ProjectStart.Value).Days;
                    var durationString = "";
                    durationString = DateTimeHelper.GetDurationString(totalDays, durationString);
                    model.Duration = durationString;
                }
                model.SectorName = entity.Sector;
                model.SubsectorName = entity.SubSectorName;
                model.ProjectName = entity.Name;
                model.ProjectValue = entity.ProjectValue;
                model.Address = entity.Address;
                model.AmountDisbursed = entity.DisbursedAmount.HasValue ? Convert.ToDecimal(entity.DisbursedAmount) : 0;

                model.Status = entity.Status.HasValue ? entity.Status.Value : 0;
                var countries = ampEntities.Database.SqlQuery<string>("Execute [GetProjectCountries] " + id);
                model.CountriesName = countries.FirstOrDefault();
                var linkedLocs = entity.TBL_LOC_Project.ToList();
                foreach (var linkedLoc in linkedLocs)
                {
                    var loc = linkedLoc.TBL_LOC;
                    ProjectDetailModel.ProjectLocModel l = new
                        ProjectDetailModel.ProjectLocModel();
                    l.AccountNumber = loc.LocAccountNo;
                    l.Added = loc.ApprovalDate.HasValue ? loc.ApprovalDate.Value.ToString("dd MMM yyyy")
                        : "";
                    l.AllocatedValue = linkedLoc.Allocation.HasValue ? linkedLoc.Allocation.Value : 0;
                    l.Classification = loc.Classification;
                    l.Country = string.Format("{0},{1}", loc.TBL_Country.Name, loc.TBL_Country.TBL_Regions.Name);
                    l.LocName = loc.Name;
                    l.LocId = loc.Id.ToString();
                    l.LinkId = linkedLoc.Id;
                    model.Locs.Add(l);
                }

                var pLocations = entity.TBL_ProjectLocations.Where(e => e.Deleted != 1).ToList();
                foreach (var pLocation in pLocations)
                {
                    ProjectDetailModel.ProjectLocationModel l = new ProjectDetailModel.ProjectLocationModel();
                    l.SequenceNumber = pLocation.SequenceNumber;
                    l.Latitude = pLocation.Latitude;
                    l.Longitude = pLocation.Longitude;
                    l.Color = pLocation.Color;
                    l.MapType = pLocation.MapType;
                    l.ProjectId = pLocation.ProjectId.ToString();
                    model.Locations.Add(l);
                }

                var timeLines = entity.TBL_ProjectTimeLines.ToList();
                foreach (var tl in timeLines)
                {
                    ProjectTimelineModel timelineModel = new ProjectTimelineModel();
                    timelineModel.Id = tl.Id;
                    timelineModel.ProjectId = tl.ProjectId.HasValue ? tl.ProjectId.Value : 0;
                    timelineModel.ProjectTimeLine = tl.TimelineTitle;
                    timelineModel.TimelineDate = tl.TimelineDate.HasValue ? tl.TimelineDate.Value.ToString("dd/MM/yyyy") : "";
                    model.TimeLines.Add(timelineModel);
                }
                model.Files = GetAllFiles(Utilities.Constants.Project, model.Id);
                model.ProjectContracts = GetContractsList(model.Id);
                foreach (var pq in entity.TBL_Projects_PQ.ToList())
                {
                    model.ProjectPQs.Add(ConvertToProjectPqModel(pq));
                }
            }
            return model;
        }

        public List<ProjectGridModel> GetProjectsForGrid(string search, bool MyRecords = false)
        {
            string sql = "Execute GETPROJECTLIST  @name";
            List<SqlParameter> param = new List<SqlParameter>();
            if (string.IsNullOrEmpty(search))
                param.Add(new SqlParameter("@name", DBNull.Value));
            else
                param.Add(new SqlParameter("@name", search));

            var list = ampEntities.Database.SqlQuery<ProjectGridModel>(sql, param.ToArray())
                .ToList();
            var user = UserConversion.Convertuser().UserId;
            if (MyRecords && user != 0)
            {
                var records = ampEntities.TBl_TeamMapping.ToList().Where(e => e.UserId == user && (string.Compare("TBL_Project", e.EntityName, StringComparison.OrdinalIgnoreCase) == 0))
                    .Select(e => e.EntityId)
                    .ToList();
                list = list.Where(e => records.Contains(e.Id)).ToList();
            }
            return list;
        }

        public DbStatus AddProject(ProjectModel model)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute AddProject @id, @name, @description, @status, @dprDate, @baselineData, "
                + "@locationAddress, @locationCordinates, @stage, @subSector, @preQualification, @authority, "
                + "@progress, @financialprogress, @projectValue, @projectStart, @projectEnd, @sector, @subsectorname, @address, @note, @physicalprogress ";
            List<SqlParameter> param = GetSqlProjectParameters(model);
            status = ExecuteSqlCommand(sql, param);

            AddTeamMember(UserConversion.Convertuser().UserId, status.ProcessedId, "TBL_Project");

            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = status.ProcessedId,
                    EntityName = "TBL_Project",
                    Message = "Created the Project",
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }

            return status;
        }

        public DbStatus UpdateProject(ProjectModel model)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute UpdateProject @id, @name, @description, @status, @dprDate, @baselineData, "
            + "@locationAddress, @locationCordinates, @stage, @subSector, @preQualification, @authority, "
            + "@progress, @financialprogress, @projectValue, @projectStart, @projectEnd, @sector, @subsectorname, @address, @note, @physicalprogress";
            List<SqlParameter> param = GetSqlProjectParameters(model);
            status = ExecuteSqlCommand(sql, param);


            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.Id,
                    EntityName = "TBL_Project",
                    Message = "Updated the Project",
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }

            return status;
        }

        public DbStatus UpdateProjectProgress(int projectId, int statusId)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute Update_ProjectProgress @projectId, @statusId ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@projectId", projectId));
            param.Add(new SqlParameter("@statusId", statusId));
            status = ExecuteSqlCommand(sql, param);

            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = projectId,
                    EntityName = "TBL_Project",
                    Message = "Updated the Project Progress",
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }

            return status;
        }

        private static List<SqlParameter> GetSqlProjectParameters(ProjectModel model)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id", model.Id));
            param.Add(new SqlParameter("@name", model.Name));
            if (string.IsNullOrEmpty(model.Description))
                param.Add(new SqlParameter("@description", DBNull.Value));
            else
                param.Add(new SqlParameter("@description", model.Description));
            if (model.Status > 0)
                param.Add(new SqlParameter("@status", model.Status));
            else
                param.Add(new SqlParameter("@status", DBNull.Value));

            if (model.DPRDate.HasValue)
                param.Add(new SqlParameter("@dprDate", model.DPRDate));
            else
                param.Add(new SqlParameter("@dprDate", DBNull.Value));

            param.Add(new SqlParameter("@baselineData", DBNull.Value));
            param.Add(new SqlParameter("@locationAddress", DBNull.Value));
            param.Add(new SqlParameter("@locationCordinates", DBNull.Value));

            if (model.Stage > 0)
                param.Add(new SqlParameter("@stage", model.Stage));
            else
                param.Add(new SqlParameter("@stage", DBNull.Value));


            if (model.SubSector > 0)
                param.Add(new SqlParameter("@subSector", model.Stage));
            else
                param.Add(new SqlParameter("@subSector", DBNull.Value));

            param.Add(new SqlParameter("@preQualification", DBNull.Value));
            param.Add(new SqlParameter("@authority", DBNull.Value));
            var temp = 0;
            param.Add(new SqlParameter("@progress", temp));
            param.Add(new SqlParameter("@financialprogress", temp));
            param.Add(new SqlParameter("@projectValue", model.ProjectValue));

            if (model.ProjectStartDate.HasValue)
                param.Add(new SqlParameter("@projectStart", model.ProjectStartDate));
            else
                param.Add(new SqlParameter("@projectStart", DBNull.Value));

            if (model.ProjectEndDate.HasValue)
                param.Add(new SqlParameter("@projectEnd", model.ProjectEndDate));
            else
                param.Add(new SqlParameter("@projectEnd", DBNull.Value));

            if (string.IsNullOrEmpty(model.Sector))
                param.Add(new SqlParameter("@sector", DBNull.Value));
            else
                param.Add(new SqlParameter("@sector", model.Sector));

            if (string.IsNullOrEmpty(model.SubSectorName))
                param.Add(new SqlParameter("@subsectorname", DBNull.Value));
            else
                param.Add(new SqlParameter("@subsectorname", model.SubSectorName));

            if (string.IsNullOrEmpty(model.Address))
                param.Add(new SqlParameter("@address", DBNull.Value));
            else
                param.Add(new SqlParameter("@address", model.Address));

            if (string.IsNullOrEmpty(model.Note))
                param.Add(new SqlParameter("@note", DBNull.Value));
            else
                param.Add(new SqlParameter("@note", model.Note));

            param.Add(new SqlParameter("@physicalprogress", model.ProjectProgress));
            return param;
        }

        //TO DO: Function body is not actula but a simulator
        public List<Contract> AddContractsToProject(List<int> contractsId)
        {
            List<Contract> contracts = new List<Contract>();
            foreach (var id in contractsId)
            {
                var contract = AppSimulatorService.GetAllContracts().Where(m => m.Id == id).FirstOrDefault();
                if (contract != null)
                {
                    //try saving the contract for project here

                    contracts.Add(contract);
                }
            }
            return contracts;
        }

        public void DeleteProject(int Id)
        {

        }

        public List<ProjectModel> ConvertToProjectModelFromEntity(List<TBL_Projects> projects)
        {
            List<ProjectModel> _projects = new List<ProjectModel>();
            foreach (var project in projects)
            {
                ProjectModel prj = new ProjectModel();

                prj.BaselineData = project.BaselineData;
                prj.Description = project.Description;
                prj.DPRDate = project.DPRDate;
                prj.ProjectValue = project.ProjectValue;
                prj.ProjectProgress = project.Progress.HasValue ? Convert.ToDecimal(project.Progress) : 0;
                prj.Id = project.Id;
                prj.Name = project.Name;
                prj.Sector = project.Sector;
                _projects.Add(prj);
            }
            return _projects;
        }

        public DbStatus UpdateProjectCode(string code, int projectId)
        {
            DbStatus status = new DbStatus();
            try
            {
                var project = ampEntities.TBL_Projects.FirstOrDefault(m => m.Code == code && m.Id != projectId);
                if (project == null)
                {
                    project = ampEntities.TBL_Projects.FirstOrDefault(m => m.Id == projectId);
                    if (project != null)
                    {
                        if (!string.IsNullOrEmpty(code))
                        {
                            project.Code = code;
                            ampEntities.SaveChanges();
                            status.Status = true;
                            status.Message = "Project code updated successfully!";
                        }
                        else
                        {
                            status.Status = false;
                            status.Message = "Project code cannot be empty.";
                        }
                    }
                    else
                    {
                        status.Status = false;
                        status.Message = "Project not found";
                    }
                }
                else
                {
                    status.Status = false;
                    status.Message = "Project code already exists. Consider a different code.";
                }
            }
            catch (Exception ex)
            {
                status.Status = false;
                status.Message = "Something went wrong while updating project code.";
            }
            return status;
        }

        #region Project Timelines
        public DbStatus AddProjectTimeline(ProjectTimelineModel model)
        {
            TBL_ProjectTimeLines projectTimeLine = new TBL_ProjectTimeLines();
            projectTimeLine.ProjectId = model.ProjectId;
            projectTimeLine.Id = model.Id;
            projectTimeLine.TimelineTitle = model.ProjectTimeLine;
            projectTimeLine.TimelineDate = DateTimeHelper.ToDateObject(model.TimelineDate);

            return AddUpdateProjectTimeline(projectTimeLine);
        }

        public void UpdateProjectTimelines(List<ProjectTimelineModel> model)
        {
            foreach (var tl in model)
            {
                AddProjectTimeline(tl);
            }
        }

        public DbStatus AddUpdateProjectTimeline(TBL_ProjectTimeLines timeline)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute AddUpdate_ProjectTimelines @id, @projectid, @timelineTitle, @timelineDate ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id", timeline.Id));
            param.Add(new SqlParameter("@projectid", timeline.ProjectId));
            param.Add(new SqlParameter("@timelineTitle", timeline.TimelineTitle));
            param.Add(new SqlParameter("@timelineDate", timeline.TimelineDate));
            status = ExecuteSqlCommand(sql, param);

            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = timeline.ProjectId.Value,
                    EntityName = "TBL_Project",
                    Message = string.Format("{0} the Project TimeLine", timeline.Id != 0 ? "Updated" : "Created"),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }

            return status;
        }
        #endregion

        #endregion

        #region LOC - PROJECTS
        public List<ProjectModel> GetUnlinkedProjectForLoc(int locId)
        {
            List<ProjectModel> projects = new List<ProjectModel>();
            var linkedProjectIds = ampEntities.TBL_LOC_Project.Where(m => m.LocId == locId).Select(m => m.ProjectId).ToList();
            var unLinkedProjects = ampEntities.TBL_Projects.Where(p => !linkedProjectIds.Contains(p.Id)).ToList();
            projects = ConvertToProjectModelFromEntity(unLinkedProjects);
            return projects;
        }

        public List<LOCModel> GetUnlinkedLocForProject(int projectId)
        {
            List<LOCModel> list = new List<LOCModel>();
            var linkedLocIds = ampEntities.TBL_LOC_Project.Where(m => m.ProjectId == projectId)
                .Select(m => m.LocId).ToList();
            var unlinkedLocs = ampEntities.TBL_LOC.Where(m => !linkedLocIds.Contains(m.Id)).ToList();
            foreach (var loc in unlinkedLocs)
            {
                list.Add(ConvertLocEntityToModel(loc));
            }
            return list;
        }

        public List<LOCoption> GetUnlinkedLocForProjectJ(int projectId)
        {
            List<LOCoption> loclist = new List<LOCoption>();
            var linkedLocIds = ampEntities.TBL_LOC_Project.Where(m => m.ProjectId == projectId)
                .Select(m => m.LocId).ToList();
            var unlinkedLocs = ampEntities.TBL_LOC.Where(m => !linkedLocIds.Contains(m.Id)).ToList();
            foreach (var loc in unlinkedLocs)
            {
                LOCoption locop = new
                LOCoption
                {
                    LOCid = loc.Id,
                    LOCName = loc.LocNumber + "-" + loc.Name + " (" + loc.LocAccountNo + ")"
                };
                loclist.Add(locop);
            }
            return loclist;
        }

        #endregion

        #region Contacts and Contact Types
        public List<TBL_ContactTypes> GetAllContactTypes()
        {
            var list = ampEntities.TBL_ContactTypes.Where(e => e.IsActive == true).ToList();
            return list;
        }
        public List<ContactModel> GetUnlinkedContactsForProject(int projectId, string contactName)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            var linkedContactIds = ampEntities.TBL_Project_Contacts.Where(m => m.ProjectId == projectId)
                .Select(m => m.ContactId).ToList();
            var unLinkedContacts = ampEntities.TBL_Contacts.Where(m => !linkedContactIds.Contains(m.Id)
            && m.Name.ToLower().Contains(contactName.ToLower()));
            foreach (var contact in unLinkedContacts)
            {
                ContactModel ct = ContactEntityToModel(contact);
                contacts.Add(ct);
            }
            return contacts;
        }
        public List<ContactModel> GetAllContactsForProject(int projectId)
        {
            List<ContactModel> contacts = new List<ContactModel>();
            var linkedContactIds = ampEntities.TBL_Project_Contacts.Where(m => m.ProjectId == projectId)
                .Select(m => m.ContactId).ToList();
            var contactList = ampEntities.TBL_Contacts.Where(m => linkedContactIds.Contains(m.Id));
            foreach (var contact in contactList)
            {
                ContactModel ct = ContactEntityToModel(contact);
                contacts.Add(ct);
            }
            return contacts;
        }
        private static ContactModel ContactEntityToModel(TBL_Contacts contact)
        {
            ContactModel ct = new ContactModel();
            ct.AddressLine1 = contact.AddressLine1;
            ct.AddressLine2 = contact.AddressLine2;
            ct.City = contact.City;
            ct.Pincode = contact.PinCode;
            ct.Organization = contact.Organization;
            ct.Name = contact.Name;
            ct.MobileNumber = contact.MobileNumber;
            ct.Landline = contact.Landline;
            ct.Id = contact.Id;
            ct.FaxNumber = contact.Faxno;
            ct.Email = contact.Email;
            ct.Designation = contact.Designation;
            ct.ContactType = contact.TBL_ContactTypes.Name;
            ct.ContactTypeId = contact.ContactTypeId.Value;
            ct.CountryId = contact.CountryId;
            ct.CountryName = contact.TBL_Country.Name;
            ct._contactImage = contact.ContactImg;
            return ct;
        }
        public ContactModel GetContactDetail(int contactId)
        {
            var contact = ampEntities.TBL_Contacts.Where(m => m.Id == contactId).FirstOrDefault();
            ContactModel model = new ContactModel();
            if (contact != null)
                model = ContactEntityToModel(contact);
            return model;
        }
        public DbStatus SaveContact(ContactModel contact)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute AddUpdateContact @contactId, @name, @landline, @mobile, @fax, @email, @addrline1, @addrline2, @city, "
                + "@pincode, @countryId, @contactTypeId, @organization, @designation ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@contactId", contact.Id));
            param.Add(StringCheckSqlParameter("@name", contact.Name));
            param.Add(StringCheckSqlParameter("@landline", contact.Landline));
            param.Add(StringCheckSqlParameter("@mobile", contact.MobileNumber));
            param.Add(StringCheckSqlParameter("@fax", contact.FaxNumber));
            param.Add(StringCheckSqlParameter("@email", contact.Email));
            param.Add(StringCheckSqlParameter("@addrline1", contact.AddressLine1));
            param.Add(StringCheckSqlParameter("@addrline2", contact.AddressLine2));
            param.Add(StringCheckSqlParameter("@city", contact.City));
            param.Add(StringCheckSqlParameter("@pincode", contact.Pincode));
            param.Add(new SqlParameter("@countryId", contact.CountryId));
            param.Add(new SqlParameter("@contactTypeId", contact.ContactTypeId));
            param.Add(StringCheckSqlParameter("@organization", contact.Organization));
            param.Add(StringCheckSqlParameter("@designation", contact.Designation));

            status = ExecuteSqlCommand(sql, param);
            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = status.ProcessedId,
                    EntityName = "TBL_Contact",
                    Message = string.Format("{0} the Project TimeLine", contact.Id != 0 ? "Updated" : "Created"),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
            return status;
        }
        public DbStatus SaveandLinkContactToProject(ContactModel contact, int projectId)
        {
            var status = SaveContact(contact);
            if (status.Status && status.ProcessedId > 0)
            {
                LinkContactToProject(new List<int> { status.ProcessedId }, projectId);
            }
            return status;
        }
        public void LinkContactToProject(List<int> contactIds, int projectId)
        {
            foreach (var id in contactIds)
            {
                string sql = "Execute LinkProjectContacts @projectId, @contactId ";
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@projectId", projectId));
                param.Add(new SqlParameter("@contactId", id));

                using (AMPEntities entity = new AMPEntities())
                {
                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = projectId,
                        EntityName = "TBL_Project",
                        Message = "Updated Contact List",
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                    entity.SaveChanges();
                }
                ExecuteSqlCommand(sql, param);
            }
        }
        public void SaveProjectLocations(List<ProjectDetailModel.Markers> locations, int projectId, string mapType)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                entity.TBL_ProjectLocations.Where(e => e.ProjectId == projectId).ToList().ForEach(a => a.Deleted = 1);
                foreach (var marker in locations)
                {
                    entity.TBL_ProjectLocations.Add(new TBL_ProjectLocations()
                    {
                        SequenceNumber = Convert.ToInt32(marker.id),
                        Latitude = marker.lat,
                        Longitude = marker.lon,
                        Color = marker.color,
                        MapType = mapType,
                        ProjectId = projectId,

                    });
                }


                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = projectId,
                    EntityName = "TBL_Project",
                    Message = "Updated Map Markers",
                    CreatedBy = UserConversion.Convertuser().UserId,
                });

                entity.SaveChanges();
            }
        }

        #endregion

        #region Contract 
        public ContractModel GetContractDetail(int contractId)
        {
            ContractModel contract = new ContractModel();
            var contractEntity = ampEntities.TBL_Contracts.Where(m => m.Id == contractId).FirstOrDefault();
            if (contractEntity != null)
            {
                contract.Id = contractEntity.Id;
                contract.CNote = contractEntity.Note;
                contract.AdvPmtGrntAmount = contractEntity.AdvPmtGrntAmount.HasValue ? contractEntity.AdvPmtGrntAmount.Value : 0;
                contract.AdvPmtGrntExpiry = contractEntity.AdvPmtGrntExpiry.HasValue ? contractEntity.AdvPmtGrntExpiry.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.CGSId = contractEntity.CGSId;
                contract.ContractStart = contractEntity.ContractStart.HasValue ? contractEntity.ContractStart.Value.ToString("dd/MM/yyyy") : "";
                contract.ContractEnd = contractEntity.ContractEnd.HasValue ? contractEntity.ContractEnd.Value.ToString("dd/MM/yyyy") : "";
                contract.CompReportDate = contractEntity.CompReportDate.HasValue ? contractEntity.CompReportDate.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.ContractorCIF = contractEntity.ContractorCIF;
                contract.ContractType = contractEntity.ContractType;
                contract.DefectsLiabilityEndDate = contractEntity.DefectsLiabilityEndDate.HasValue ? contractEntity.DefectsLiabilityEndDate.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.Description = contractEntity.Description;
                contract.FACDate = contractEntity.FACDate.HasValue ? contractEntity.FACDate.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.OtherGrntAmount = contractEntity.OtherGrntAmount.HasValue ? contractEntity.OtherGrntAmount.Value : 0;
                contract.OtherGrntExpiry = contractEntity.OtherGrntExpiry.HasValue ? contractEntity.OtherGrntExpiry.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.GuaranteeNote = contractEntity.GuaranteeNote;
                contract.PACDate = contractEntity.PACDate.HasValue ? contractEntity.PACDate.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.PerBankGrntAmount = contractEntity.PerBankGrntAmount.HasValue ? contractEntity.PerBankGrntAmount.Value : 0;
                contract.PerBankGrntExpiry = contractEntity.PerBankGrntExpiry.HasValue ? contractEntity.PerBankGrntExpiry.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.PQId = contractEntity.PQId;
                contract.TenderIssueDate = contractEntity.TenderIssueDate.HasValue ? contractEntity.TenderIssueDate.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.TenderLastDate = contractEntity.TenderLastDate.HasValue ? contractEntity.TenderLastDate.Value.ToString("dd/MM/yyyy") : ""; ;
                contract.ContractId = contractEntity.PackageId;
                contract.ContractorName = contractEntity.ContractorName;
                contract.PackageDisplayId = contractEntity.PackageDisplayId;
                contract.TypeOfPackage = contractEntity.TypeOfPackage;
                contract.EstimateValue = contractEntity.EstimateValue.HasValue ? contractEntity.EstimateValue.Value : 0;
                contract.ProjectPqDetail = GetProjectPqDetail(contract.PQId);
                contract.ProjectPqDetail.ContractId = contract.Id;
                contract.Files = GetAllFiles(Utilities.Constants.Contract, contract.Id);
                contract.LocAmount = contractEntity.LocAmount;
                contract.BorowerGovt = contractEntity.GovtAmount;
                contract.OtherSources = contractEntity.OtherAmount;
                contract.DurationYear = contractEntity.DurationYear.HasValue ? contractEntity.DurationYear.Value : 0;
                contract.DurationMonth = contractEntity.DurationMonth.HasValue ? contractEntity.DurationMonth.Value : 0;
                contract.DurationDay = contractEntity.DurationDay.HasValue ? contractEntity.DurationDay.Value : 0;
                contract.Project = contractEntity.TBL_Projects_PQ.TBL_Projects;
                contract.ScheduledCompDate = contractEntity.ScheduledCompDate.HasValue ? contractEntity.ScheduledCompDate.Value.ToString("dd/MM/yyyy") : "";
                contract.ContractApprovalDate = contractEntity.ContractApprovalDate.HasValue ? contractEntity.ContractApprovalDate.Value.ToString("dd/MM/yyyy") : "";
                contract.RevisedCompletionDate = contractEntity.RevisedCompletionDate.HasValue ? contractEntity.RevisedCompletionDate.Value.ToString("dd/MM/yyyy") : "";
                contract.ActualCompletionDate = contractEntity.ActualCompletionDate.HasValue ? contractEntity.ActualCompletionDate.Value.ToString("dd/MM/yyyy") : "";
                contract.TerminalDateOfDisbursement = contractEntity.TerminalDateOfDisbursement.HasValue ? contractEntity.TerminalDateOfDisbursement.Value.ToString("dd/MM/yyyy") : "";
                contract.DateOfReceiptOfContractByEximBank = contractEntity.DateOfReceiptOfContractByEximBank.HasValue ? contractEntity.DateOfReceiptOfContractByEximBank.Value.ToString("dd/MM/yyyy") : "";
                contract.SigningDate = contractEntity.SigningDate.HasValue ? contractEntity.SigningDate.Value.ToString("dd/MM/yyyy") : "";
                contract.SignEffectiveDate = contractEntity.SignEffectiveDate.HasValue ? contractEntity.SignEffectiveDate.Value.ToString("dd/MM/yyyy") : "";


                contract.Applicants = GetApplicants(contract.PQId);

                contract.Responsibility = ampEntities.TBL_ContractResponsibility.Where(e => e.ContractId == contractEntity.Id).ToList()
                    .Select(e =>
                    {
                        return new ResponsibilityModel()
                        {
                            Authority = e.Authority,
                            ContractId = e.ContractId,
                            Id = e.Id,
                            Responsiblity = e.Responsiblity
                        };
                    }).ToList();

                contract.PaymentTerms = ampEntities.TBL_ContractTerms.ToList().Where(e => e.ContractId == contractEntity.Id).Select(x =>
                {
                    return new PaymentTermsModel()
                    {
                        Id = x.Id,
                        ContractId = x.ContractId,
                        Milestone = x.Milestone,
                        Percentage = x.Percentage,
                        Amount = x.Amount,
                        Note = x.Note,
                        Sequence = x.Sequence
                    };
                }).OrderBy(e => e.Sequence).ToList();

                contract.Content = ampEntities.TBL_ContractContent.ToList().Where(e => e.ContractId == contractEntity.Id).Select(x =>
                {

                    return new ContentRequirementModel()
                    {
                        isExempt = x.isExempt,

                        MEAApprovalDate = x.MEAApprovalDate.HasValue ? x.MEAApprovalDate.Value.ToString("dd/MM/yyyy") : "",
                        MEAApprovalRefNo = x.MEAApprovalRefNo,
                        Percentage = x.Percentage,
                        ContractId = x.ContractId,
                        Remarks = x.Remarks,
                        RevisedRequirement = x.RevisedRequirement,
                        Value = x.Value,
                        Id = x.Id,
                        Type = x.Type

                    };
                }).ToList();

                contract.LetterOfCredits = ampEntities.TBL_ContractLC.ToList().Where(e => e.ContractId == contractEntity.Id).Select(x =>
                {
                    return new CreditLetterModel()
                    {
                        Amount = x.Amount,
                        TransferableAmount = x.TransferableAmount,
                        Beneficiary = x.Beneficiary,
                        ContractId = x.ContractId,
                        ExpiryDate = x.ExpiryDate.HasValue ? x.ExpiryDate.Value.ToString("dd/MM/yyyy") : "",
                        IssuingBank = x.IssuingBank,
                        Id = x.Id,
                        LastDateofShipment = x.LastDateofShipment.HasValue ? x.LastDateofShipment.Value.ToString("dd/MM/yyyy") : "",
                        LCNumber = x.LCNumber,
                        OpeningDate = x.OpeningDate.HasValue ? x.OpeningDate.Value.ToString("dd/MM/yyyy") : "",
                        LCNote = x.LCNote
                    };
                }).ToList();

                contract.Locs = ampEntities.TBL_ContractLocMap.ToList().Where(e => e.ContractId == contractEntity.Id).Select(x =>
                {
                    return new LocMapModel()
                    {
                        DateAdded = x.CreatedOn,
                        Loc = GetLocById(x.LocId),
                        Value = x.Value,
                        ContractId = x.ContractId,
                        LocId = x.LocId,

                    };
                }).ToList();

                #region Finacle

                using (AMPEntities entity = new AMPEntities())
                {
                    var disbamnt = entity.Finacle_Contract_Transanctions.Where(e => e.AccountId.Equals(contract.CGSId)).Select(a => a.CummulativeCredit).FirstOrDefault();
                    contract.AmountDisbursed = disbamnt.HasValue ? Convert.ToDecimal(disbamnt) : 0;
                }

                #endregion
            }
            return contract;
        }

        public void UpdateContractTenure(ContractModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {

                var record = entity.TBL_Contracts.Where(e => e.Id == model.Id).FirstOrDefault();
                if (record != null)
                {
                    var date = DateTimeHelper.ToDateObject(model.TenderIssueDate);
                    if (date.HasValue)
                    {
                        record.TenderIssueDate = date.Value;

                    }
                    date = DateTimeHelper.ToDateObject(model.TenderLastDate);
                    if (date.HasValue)
                    {
                        record.TenderLastDate = date.Value;
                    }
                    // record.TenderIssueDate = model.TenderIssueDate;
                    // record.TenderLastDate = model.TenderLastDate;

                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = model.Id,
                        EntityName = "TBL_Contract",
                        Message = string.Format("Updated the Tender"),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }
                entity.SaveChanges();
            }
        }
        public void UpdateContractFunding(ContractModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var record = entity.TBL_Contracts.Where(e => e.Id == model.Id).FirstOrDefault();
                if (record != null)
                {
                    record.LocAmount = model.LocAmount;
                    record.GovtAmount = model.BorowerGovt;
                    record.OtherAmount = model.OtherSources;

                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = model.Id,
                        EntityName = "TBL_Contract",
                        Message = string.Format("Updated the Contract Funding"),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }
                entity.SaveChanges();
            }
        }

        public void UpdateContractClosure(ContractModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var record = entity.TBL_Contracts.Where(e => e.Id == model.Id).FirstOrDefault();
                if (record != null)
                {
                    var date = DateTimeHelper.ToDateObject(model.PACDate);
                    if (date.HasValue)
                    {
                        record.PACDate = date.Value;
                    }
                    date = DateTimeHelper.ToDateObject(model.DefectsLiabilityEndDate);
                    if (date.HasValue)
                    {
                        record.DefectsLiabilityEndDate = date.Value;
                    }
                    date = DateTimeHelper.ToDateObject(model.FACDate);
                    if (date.HasValue)
                    {
                        record.FACDate = date.Value;
                    }
                    date = DateTimeHelper.ToDateObject(model.CompReportDate);
                    if (date.HasValue)
                    {
                        record.CompReportDate = date.Value;
                    }
                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = model.Id,
                        EntityName = "TBL_Contract",
                        Message = string.Format("Updated the Contract Closure"),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }

                entity.SaveChanges();
            }
        }

        public void UpdateContractGuarantee(ContractModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var record = entity.TBL_Contracts.Where(e => e.Id == model.Id).FirstOrDefault();
                if (record != null)
                {

                    record.AdvPmtGrntAmount = model.AdvPmtGrntAmount;
                    var date = DateTimeHelper.ToDateObject(model.AdvPmtGrntExpiry);
                    if (date.HasValue)
                    {
                        record.AdvPmtGrntExpiry = date.Value;

                    }
                    //record.AdvPmtGrntExpiry = model.AdvPmtGrntExpiry;
                    record.PerBankGrntAmount = model.PerBankGrntAmount;
                    date = DateTimeHelper.ToDateObject(model.PerBankGrntExpiry);
                    if (date.HasValue)
                    {
                        record.PerBankGrntExpiry = date.Value;

                    }
                    // record.PerBankGrntExpiry = model.PerBankGrntExpiry;
                    record.OtherGrntAmount = model.OtherGrntAmount;
                    date = DateTimeHelper.ToDateObject(model.OtherGrntExpiry);
                    if (date.HasValue)
                    {
                        record.OtherGrntExpiry = date.Value;

                    }
                    // record.OtherGrntExpiry = model.OtherGrntExpiry;
                    record.GuaranteeNote += "</br>" + DateTime.Now.ToShortDateString() + "-" + model.GuaranteeNote;
                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = model.Id,
                        EntityName = "TBL_Contract",
                        Message = string.Format("Updated the Contract Guarantee"),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }

                entity.SaveChanges();
            }
        }

        public DbStatus SaveContract(ContractModel contractModel)
        {
            DbStatus dbStatus = new DbStatus();
            string sql = "Execute AddUpdate_Contract @Id, @ContractName, @TypeOfPackage, @EstimateValue, @ContractorCIF, @CGS, "
                + "@PqId, @start, @end, @note, @scheduledDate,@ContractApprovalDate,@RevisedCompletionDate,@ActualCompletionDate,"
                + "@TerminalDateOfDisbursement,@DateOfReceiptOfContractByEximBank, @SiginingDate, @SignEffectiveDate,@DurationYear,@DurationMonth,@DurationDay ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Id", contractModel.Id));
            param.Add(new SqlParameter("@ContractName", contractModel.ContractorName));
            param.Add(new SqlParameter("@TypeOfPackage", contractModel.ContractType));
            param.Add(new SqlParameter("@EstimateValue", contractModel.EstimateValue));
            param.Add(new SqlParameter("@ContractorCIF", contractModel.ContractorCIF));
            param.Add(new SqlParameter("@CGS", contractModel.CGSId));
            param.Add(new SqlParameter("@PqId", contractModel.PQId));
            param.Add(new SqlParameter("@DurationYear", contractModel.DurationYear));
            param.Add(new SqlParameter("@DurationMonth", contractModel.DurationMonth));
            param.Add(new SqlParameter("@DurationDay", contractModel.DurationDay));
            var date = DateTimeHelper.ToDateObject(contractModel.ContractStart);
            if (date.HasValue)
                param.Add(new SqlParameter("@start", date));
            else
                param.Add(new SqlParameter("@start", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.ContractEnd);
            if (date.HasValue)
                param.Add(new SqlParameter("@end", date));
            else
                param.Add(new SqlParameter("@end", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.ContractApprovalDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@ContractApprovalDate", date));
            else
                param.Add(new SqlParameter("@ContractApprovalDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.RevisedCompletionDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@RevisedCompletionDate", date));
            else
                param.Add(new SqlParameter("@RevisedCompletionDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.ActualCompletionDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@ActualCompletionDate", date));
            else
                param.Add(new SqlParameter("@ActualCompletionDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.TerminalDateOfDisbursement);
            if (date.HasValue)
                param.Add(new SqlParameter("@TerminalDateOfDisbursement", date));
            else
                param.Add(new SqlParameter("@TerminalDateOfDisbursement", DBNull.Value));
            date = DateTimeHelper.ToDateObject(contractModel.DateOfReceiptOfContractByEximBank);
            if (date.HasValue)
                param.Add(new SqlParameter("@DateOfReceiptOfContractByEximBank", date));
            else
                param.Add(new SqlParameter("@DateOfReceiptOfContractByEximBank", DBNull.Value));


            if (string.IsNullOrEmpty(contractModel.CNote))
                param.Add(new SqlParameter("@note", DBNull.Value));
            else
                param.Add(new SqlParameter("@note", contractModel.CNote));
            //date = DateTimeHelper.ToDateObject(model.TerminalDate);
            //if (date.HasValue)
            //    param.Add(new SqlParameter("@terminalDate", date));
            //else
            //    param.Add(new SqlParameter("@terminalDate", DBNull.Value));
            //contractModel.ScheduledCompDate = DateTimeHelper.ToDateObject(contractModel)
            date = DateTimeHelper.ToDateObject(contractModel.ScheduledCompDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@scheduledDate", date));
            else
                param.Add(new SqlParameter("@scheduledDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.SigningDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@SiginingDate", date));
            else
                param.Add(new SqlParameter("@SiginingDate", DBNull.Value));

            date = DateTimeHelper.ToDateObject(contractModel.SignEffectiveDate);
            if (date.HasValue)
                param.Add(new SqlParameter("@SignEffectiveDate", date));
            else
                param.Add(new SqlParameter("@SignEffectiveDate", DBNull.Value));

            dbStatus = ExecuteSqlCommand(sql, param);
            contractModel.Id = dbStatus.ProcessedId;

            using (AMPEntities entity = new AMPEntities())
            {
                if (contractModel.Id != 0)
                {
                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = contractModel.Id,
                        EntityName = "TBL_Contract",
                        Message = string.Format("Updated the contract"),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }
                else
                {
                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = contractModel.PQId,
                        EntityName = "TBL_Projects_PQ",
                        Message = string.Format("Added the contract {0}", contractModel.ContractorName),
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }

                entity.SaveChanges();
            }
            return dbStatus;
        }

        public DbStatus UpdateContractNumber(string code, int contractId)
        {
            DbStatus status = new DbStatus();
            try
            {
                var contract = ampEntities.TBL_Contracts.FirstOrDefault(m => m.PackageId == code && m.Id != contractId);
                if (contract == null)
                {
                    contract = ampEntities.TBL_Contracts.FirstOrDefault(m => m.Id == contractId);
                    if (contract != null)
                    {
                        if (!string.IsNullOrEmpty(code))
                        {
                            contract.PackageId = code;
                            ampEntities.SaveChanges();
                            status.Status = true;
                            status.Message = "Contract number updated successfully!";
                        }
                        else
                        {
                            status.Status = false;
                            status.Message = "Contract number cannot be empty.";
                        }
                    }
                    else
                    {
                        status.Status = false;
                        status.Message = "Contract not found";
                    }
                }
                else
                {
                    status.Status = false;
                    status.Message = "Contract number already exists. Consider a different code.";
                }
            }
            catch (Exception ex)
            {
                status.Status = false;
                status.Message = "Something went wrong while updating contract number.";
            }
            return status;
        }

        public List<ApplicantsModel> GetApplicants(int pqId)
        {
            var list = ampEntities.Tbl_Applicants
                .Join(ampEntities.TBL_Options, appl => appl.Status, opt => opt.Id, (appl, opt) => new { Applicants = appl, Options = opt })
                .Where(m => m.Applicants.PqId == pqId)
                .Select(m => new ApplicantsModel
                {
                    Id = m.Applicants.Id,
                    ApplicantNo = m.Applicants.ApplicantNo,
                    Name = m.Applicants.Name,
                    PqId = m.Applicants.PqId.HasValue ? m.Applicants.PqId.Value : 0,
                    Organization = m.Applicants.Organization,
                    StatusId = m.Applicants.Status.HasValue ? m.Applicants.Status.Value : 0,
                    Status = m.Options.Value
                }).ToList();
            return list;
        }

        public DbStatus AddApplicant(ApplicantsModel model)
        {
            DbStatus dbStatus = new DbStatus();
            Tbl_Applicants applicant = new Tbl_Applicants();
            try
            {
                applicant.Status = model.StatusId;
                applicant.PqId = model.PqId;
                applicant.ApplicantNo = model.ApplicantNo;
                applicant.Name = model.Name;
                applicant.Organization = model.Organization;
                ampEntities.Tbl_Applicants.Add(applicant);
                ampEntities.SaveChanges();
                dbStatus.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                dbStatus.Status = true;
            }
            catch
            {
                dbStatus.Message = StringMapperService.GetValue("GeneralUpdateError");
                dbStatus.Status = false;
            }
            return dbStatus;
        }

        public DbStatus UpdateApplicant(ApplicantsModel model)
        {
            DbStatus dbStatus = new DbStatus();
            Tbl_Applicants applicant;
            if (model.Id > 0)
            {
                applicant = ampEntities.Tbl_Applicants.Where(m => m.Id == model.Id).FirstOrDefault();
                if (applicant != null)
                {
                    applicant.Status = model.StatusId;
                    applicant.Name = model.Name;
                    applicant.Organization = model.Organization;
                    try
                    {
                        ampEntities.SaveChanges();
                        dbStatus.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                        dbStatus.Status = true;
                    }
                    catch
                    {
                        dbStatus.Message = StringMapperService.GetValue("GeneralUpdateError");
                        dbStatus.Status = false;
                    }
                }
            }
            return dbStatus;
        }



        #region Responsibility
        public void AddResponsibility(ResponsibilityModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                entity.TBL_ContractResponsibility.Add(new TBL_ContractResponsibility()
                {
                    Authority = model.Authority,
                    ContractId = model.ContractId,
                    Responsiblity = model.Responsiblity,

                });

                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.ContractId,
                    EntityName = "TBL_Contract",
                    Message = string.Format("Added Responsibility {0}", model.Responsiblity),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
        }
        public void DeleteResponsibility(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                //set is delete
                entity.SaveChanges();
            }

        }
        #endregion

        #region Terms
        public void AddContractPaymentTerms(PaymentTermsModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                if (model.Id != 0)
                {
                    var record = entity.TBL_ContractTerms.FirstOrDefault(e => e.Id == model.Id);
                    if (record != null)
                    {
                        record.Milestone = model.Milestone;
                        record.Percentage = model.Percentage;
                        record.Amount = model.Amount;
                        record.Sequence = model.Sequence;
                        record.Note = model.Note;

                        //Activity
                        entity.TBL_Activity.Add(new TBL_Activity()
                        {
                            CreatedOn = DateTime.Now,
                            EntityId = model.ContractId,
                            EntityName = "TBL_Contract",
                            Message = "Updated Payment Temrs",
                            CreatedBy = UserConversion.Convertuser().UserId,
                        });
                    }
                }
                else
                {
                    entity.TBL_ContractTerms.Add(new TBL_ContractTerms()
                    {
                        ContractId = model.ContractId,
                        Milestone = model.Milestone,
                        Percentage = model.Percentage,
                        Amount = model.Amount,
                        Sequence = model.Sequence,
                        Note = model.Note
                    });
                    //Activity
                    entity.TBL_Activity.Add(new TBL_Activity()
                    {
                        CreatedOn = DateTime.Now,
                        EntityId = model.ContractId,
                        EntityName = "TBL_Contract",
                        Message = "Added Payment Terms",
                        CreatedBy = UserConversion.Convertuser().UserId,
                    });
                }

                entity.SaveChanges();
            }
        }
        public void DeleteContractPaymentTerms(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                //set is delete

                entity.SaveChanges();
            }

        }
        #endregion

        #region Content Requirement
        public void AddContractContent(ContentRequirementModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var date = DateTimeHelper.ToDateObject(model.MEAApprovalDate);
                entity.TBL_ContractContent.Add(new TBL_ContractContent()
                {
                    ContractId = model.ContractId,
                    isExempt = model.isExempt,
                    MEAApprovalDate = date,
                    MEAApprovalRefNo = model.MEAApprovalRefNo,
                    Percentage = model.Percentage,
                    Remarks = model.Remarks,
                    RevisedRequirement = model.RevisedRequirement,
                    Type = model.Type,
                    Value = model.Value
                });

                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.ContractId,
                    EntityName = "TBL_Contract",
                    Message = string.Format("Added Content Requirement"),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
        }
        public void DeleteContractContent(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                //set is delete

                entity.SaveChanges();
            }

        }
        #endregion

        #region Letter OF Credit
        public void AddContractLC(CreditLetterModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var date = DateTimeHelper.ToDateObject(model.ExpiryDate);
                var date1 = DateTimeHelper.ToDateObject(model.LastDateofShipment);
                var date2 = DateTimeHelper.ToDateObject(model.OpeningDate);
                entity.TBL_ContractLC.Add(new TBL_ContractLC()
                {
                    Amount = model.Amount,
                    TransferableAmount = model.TransferableAmount,
                    Beneficiary = model.Beneficiary,
                    ContractId = model.ContractId,
                    ExpiryDate = date,
                    IssuingBank = model.IssuingBank,
                    LastDateofShipment = date1,
                    LCNumber = model.LCNumber,
                    OpeningDate = date2,
                    LCNote = model.LCNote,
                });

                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.ContractId,
                    EntityName = "TBL_Contract",
                    Message = string.Format("Added Letter of Credit"),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
        }
        public void DeleteContractLC(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                //set is delete

                entity.SaveChanges();
            }

        }
        #endregion

        #region LOC
        public void AddContractLoc(LocMapModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                entity.TBL_ContractLocMap.Add(new TBL_ContractLocMap()
                {
                    ContractId = model.ContractId,
                    CreatedOn = DateTime.Now,
                    LocId = model.LocId,
                    Value = model.Value

                });

                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = model.ContractId,
                    EntityName = "TBL_Contract",
                    Message = string.Format("Added LOC {0}", model.Loc.LOCName),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
        }
        public void DeleteContractLOC(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                //set is delete

                entity.SaveChanges();
            }

        }
        #endregion

        #endregion

        #region BobEProcure_Packages
        public List<PackageGridModel> GetPackagesByPqNumber(string pqnumber)
        {
            List<PackageGridModel> list = new List<PackageGridModel>();

            var entities = ampEntities.BobEProcure_Packages.Where(m => m.PQNo == pqnumber).ToList();
            foreach (var obj in entities)
            {
                PackageGridModel pgm = new PackageGridModel();
                pgm.PackageId = string.Join("-", obj.PackageId.Split('-').Skip(1));
                pgm.PackageName = obj.PackageName.Trim();
                pgm.PackageDisplayId = obj.PackageDisplayId;
                pgm.TypeOfPackage = obj.TypeOfPackage;
                pgm.EstimateValue = obj.EstimateValue.HasValue ? obj.EstimateValue.Value : 0;
                pgm.Country = obj.Country;
                pgm.LocAmount = obj.LocAmount.HasValue ? obj.LocAmount.Value : 0;
                pgm.LocNumber = obj.LocNumber;
                pgm.NoOfPackage = obj.NoOfPackage.HasValue ? obj.NoOfPackage.Value : 0;
                pgm.PQId = obj.PQId;
                pgm.PQNo = obj.PQNo;
                pgm.Ref = obj.Ref;
                pgm.Status = obj.Status.HasValue ? obj.Status.Value : 0;
                pgm.Title = obj.Title;
                list.Add(pgm);
            }
            return list;
        }
        #endregion

        #region ProjectpQ Detail
        public ProjectPqModel GetProjectPqDetail(int pqId)
        {
            var detail = ampEntities.TBL_Projects_PQ.Where(m => m.Id == pqId).FirstOrDefault();
            ProjectPqModel pqModel = ConvertToProjectPqModel(detail);
            return pqModel;
        }
        public ProjectPqModel GetProjectPqPageDetail(int pqId)
        {
            var detail = ampEntities.TBL_Projects_PQ.AsNoTracking().Where(m => m.Id == pqId).FirstOrDefault();
            ProjectPqModel pqModel = ConvertToProjectPqModel(detail);
            pqModel.PQContracts = GetContractsList(0, pqId);
            pqModel.Applicants = GetApplicants(pqId);
            pqModel.Files = GetAllFiles(new string[] { Utilities.Constants.PQ_File, Utilities.Constants.PQ_DPR }, pqId);
            return pqModel;
        }
        public DbStatus AddProjectPqDetail(ProjectPqModel pqModel)
        {
            DbStatus status = new DbStatus();
            TBL_Projects_PQ detail = new TBL_Projects_PQ();
            detail.PQRefNumber = pqModel.PQRefNumber;
            detail.addendum_refno = pqModel.AddEndumRefno;
            detail.Category = pqModel.Category;
            detail.ApplicationStart = DateTimeHelper.ToDateObject(pqModel.ApplicationStart);
            detail.ApplicationEnd = DateTimeHelper.ToDateObject(pqModel.ApplicationEnd);
            detail.Status = pqModel.Status;
            detail.LastSubmissionOn = DateTimeHelper.ToDateObject(pqModel.LastSubmissionOn);
            detail.PqDocPublishedOn = DateTimeHelper.ToDateObject(pqModel.PqDocPublishedOn);
            detail.ProjectCost = pqModel.ProjectCost;
            detail.ProjectId = pqModel.ProjectId;
            detail.pq_status = pqModel.StatusId;
            detail.ProjectPQSigninDate = DateTimeHelper.ToDateObject(pqModel.ProjectPQSigninDate);
            ampEntities.TBL_Projects_PQ.Add(detail);
            try
            {
                ampEntities.SaveChanges();
                status.ProcessedId = detail.Id;
                AddActivity(status.ProcessedId, "TBL_Projects_PQ", "PQ Created.");
                status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                status.Status = true;
            }
            catch (Exception ex)
            {
                status.Message = StringMapperService.GetValue("GeneralUpdateError");
                status.Status = false;
                InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "AddProjectPqDetail"
                });
            }

            return status;
        }

        private static ProjectPqModel ConvertToProjectPqModel(TBL_Projects_PQ detail)
        {
            ProjectPqModel pqModel = new ProjectPqModel();
            if (detail != null)
            {
                pqModel.Id = detail.Id;
                pqModel.Name = detail.Name;
                pqModel.AddEndumRefno = detail.addendum_refno;
                pqModel.ApplicationEnd = detail.ApplicationEnd.HasValue ? detail.ApplicationEnd.Value.ToString("dd/MM/yyyy") : "";
                pqModel.ApplicationStart = detail.ApplicationStart.HasValue ? detail.ApplicationStart.Value.ToString("dd/MM/yyyy") : "";
                pqModel.Description = detail.Description;
                pqModel.PQRefNumber = detail.PQRefNumber;
                pqModel.StatusId = detail.pq_status.HasValue ? detail.pq_status.Value : 0;
                var option = new Dashboard2ServiceLayer().GetOptionById(pqModel.StatusId);
                pqModel.PqStatus = option == null ? "" : option.Value;
                pqModel.ProjectId = detail.ProjectId;
                pqModel.Category = detail.Category;
                pqModel.LastSubmissionOn = detail.LastSubmissionOn.HasValue ? detail.LastSubmissionOn.Value.ToString("dd/MM/yyyy") : "";
                pqModel.PqDocPublishedOn = detail.PqDocPublishedOn.HasValue ? detail.PqDocPublishedOn.Value.ToString("dd/MM/yyyy") : "";
                pqModel.ProjectCost = detail.ProjectCost.HasValue ? detail.ProjectCost.Value : 0;
                pqModel.Status = detail.Status;
                pqModel.ProjectPQSigninDate = detail.ProjectPQSigninDate.HasValue ? detail.ProjectPQSigninDate.Value.ToString("dd/MM/yyyy") : ""; 

            }

            return pqModel;
        }

        public DbStatus UpdateProjectPqDetail(ProjectPqModel pqModel)
        {
            DbStatus status = new DbStatus();
            var detail = ampEntities.TBL_Projects_PQ.Where(m => m.Id == pqModel.Id).FirstOrDefault();
            if (detail != null)
            {
                detail.PQRefNumber = pqModel.PQRefNumber;
                detail.addendum_refno = pqModel.AddEndumRefno;
                detail.Category = pqModel.Category;
                detail.ApplicationStart = DateTimeHelper.ToDateObject(pqModel.ApplicationStart);
                detail.ApplicationEnd = DateTimeHelper.ToDateObject(pqModel.ApplicationEnd);
                detail.Status = pqModel.Status;
                detail.LastSubmissionOn = DateTimeHelper.ToDateObject(pqModel.LastSubmissionOn);
                detail.PqDocPublishedOn = DateTimeHelper.ToDateObject(pqModel.PqDocPublishedOn);
                detail.pq_status = pqModel.StatusId;
                detail.ProjectCost = pqModel.ProjectCost;
                detail.ProjectPQSigninDate = DateTimeHelper.ToDateObject(pqModel.ProjectPQSigninDate);
                ampEntities.SaveChanges();
                status.ProcessedId = detail.Id;
                AddActivity(status.ProcessedId, "TBL_Projects_PQ", "PQ Details Updated.");
                status.Status = true;
                status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
            }
            else
            {
                status.Message = StringMapperService.GetValue("DataNotFoundError");
                status.Status = false;
            }
            return status;
        }
        #endregion

        #region Project-Contract
        public List<ProjectContractModel> GetContractsList(int projectId, int pqId = 0)
        {
            string sql = "Execute GetProjectContracts  @projectId, @pqId";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@projectId", projectId));
            param.Add(new SqlParameter("@pqId", pqId));
            var list = ampEntities.Database.SqlQuery<ProjectContractModel>(sql, param.ToArray())
                .ToList();
            return list;
        }

        public DbStatus UpdateProjectContracts(List<PackageGridModel> packages, int projectId)
        {
            DbStatus dbStatus = new DbStatus();
            foreach (var package in packages)
            {
                string sql = "Execute Update_Project_Contracts @ProjectId, @PackageId, @PackageName, @PackageDisplayId, @EstimateValue, @TypeOfPackage, @PQNo, "
                + "@PQId, @Ref, @Title, @Country, @Status, @LocNumber, @LocAmount, @NoOfPackage ";
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ProjectId", projectId));
                param.Add(new SqlParameter("@PackageId", package.PackageId));
                param.Add(new SqlParameter("@PackageName", package.PackageName));
                param.Add(new SqlParameter("@PackageDisplayId", package.PackageDisplayId));
                param.Add(new SqlParameter("@EstimateValue", package.EstimateValue));
                param.Add(new SqlParameter("@TypeOfPackage", package.TypeOfPackage));
                param.Add(new SqlParameter("@PQNo", package.PQNo));
                param.Add(new SqlParameter("@PQId", package.PQId));
                param.Add(new SqlParameter("@Ref", package.Ref));
                param.Add(new SqlParameter("@Title", package.Title));
                param.Add(new SqlParameter("@Country", package.Country));
                param.Add(new SqlParameter("@Status", package.Status));
                param.Add(new SqlParameter("@LocNumber", package.LocNumber));
                param.Add(new SqlParameter("@LocAmount", package.LocAmount));
                param.Add(new SqlParameter("@NoOfPackage", package.NoOfPackage));
                dbStatus = ExecuteSqlCommand(sql, param);
            }
            using (AMPEntities entity = new AMPEntities())
            {
                //Activity
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = projectId,
                    EntityName = "TBL_Project",
                    Message = string.Format("Added Contracts:  {0}", string.Join(", ", packages.Select(e => e.PackageName).ToList())),
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
            return dbStatus;
        }

        #endregion

        #region Files
        public List<TBL_Files> GetAllFiles(string fileFor, int recordId)
        {
            var files = ampEntities.TBL_Files.AsNoTracking().Where(m => m.FileFor == fileFor && m.RecordId == recordId).ToList();
            return files;
        }
        public List<TBL_Files> GetAllFiles(string[] fileFor, int recordId)
        {
            var files = ampEntities.TBL_Files.AsNoTracking().Where(m => fileFor.Contains(m.FileFor) && m.RecordId == recordId).ToList();
            return files;
        }
        public List<TBL_Files> GetAllFiles(int recordId)
        {
            var files = ampEntities.TBL_Files.AsNoTracking().Where(m => m.RecordId == recordId).ToList();
            return files;
        }

        public DbStatus AddFile(TBL_Files file)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute AddFiles @src, @fileFor, @recordId, @displayname, @userid ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@src", file.Src));
            param.Add(new SqlParameter("@fileFor", file.FileFor));
            param.Add(new SqlParameter("@recordId", file.RecordId));
            param.Add(new SqlParameter("@displayname", file.DisplayName));
            param.Add(new SqlParameter("@userid", UserConversion.Convertuser().UserId));
            try
            {
                status = ampEntities.Database.SqlQuery<DbStatus>(sql, param.ToArray()).DefaultIfEmpty(new DbStatus()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                status.Status = false;
                status.Message = StringMapperService.GetValue("GeneralUpdateError");
                InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "AddFile"
                });
            }
            return status;
        }
        public DbStatus DeleteFile(int id)
        {
            DbStatus status = new DbStatus();
            try
            {
                var file = ampEntities.TBL_Files.Where(m => m.Id == id).FirstOrDefault();
                if (file != null)
                    ampEntities.TBL_Files.Remove(file);
                status.Status = true;
                ampEntities.SaveChanges();
                status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
            }
            catch (Exception ex)
            {
                status.Status = false;
                status.Message = StringMapperService.GetValue("GeneralUpdateError");
                InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "DeleteFile"
                });
            }
            return status;
        }

        #endregion

        #region Data Adapter
        DataSet ExecuteProcedure(string ProcedureName, List<ProcedureParams> Params)
        {
            SqlConnection conn = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            var parameters = Params.Select(x =>
            {
                return new SqlParameter(string.Format("@{0}", x.Name), x.Type)
                {
                    Value = x.Value
                };

            }).ToArray();
            cmd.Parameters.AddRange(parameters);
            cmd.CommandText = ProcedureName;
            da.SelectCommand = cmd;

            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);
                conn.Close();
            }
            catch (Exception ex)
            {
                return new DataSet();
            }
            return ds;

        }

        private DbStatus ExecuteSqlCommand(string sql, List<SqlParameter> param)
        {
            DbStatus status = new DbStatus();
            try
            {
                status = ampEntities.Database.SqlQuery<DbStatus>(sql, param.ToArray()).DefaultIfEmpty(new DbStatus()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                status = new DbStatus();
                InsertLog(new LogsModel
                {
                    FullMessage = ex.Message,
                    Message = ex.Source,
                    ServiceName = "ExecuteSqlCommand"
                });
            }
            return status;
        }

        private SqlParameter StringCheckSqlParameter(string parameterName, string parameterValue)
        {
            SqlParameter sqlParameter;
            if (string.IsNullOrEmpty(parameterValue))
                sqlParameter = new SqlParameter(parameterName, DBNull.Value);
            else
                sqlParameter = new SqlParameter(parameterName, parameterValue);
            return sqlParameter;
        }
        #endregion

        #region Common Master Data
        public List<TBL_Classifications> GetAllClassification()
        {
            return ampEntities.TBL_Classifications.ToList();
        }
        public List<TBL_Status> GetAllStatus()
        {
            return ampEntities.TBL_Status.ToList();
        }
        public List<string> GetCountryAlphabetLetters()
        {
            string sql = "Execute Get_Country_Alphabets";

            var list = ampEntities.Database.SqlQuery<string>(sql)
                .ToList();
            return list;
        }
        #endregion

        #region Options
        public List<Options> GetAllOptions(string Search = "", OptionTypes type = OptionTypes.ContactType)
        {
            var list = ampEntities.TBL_Options.ToList().Where(e => (OptionTypes)e.Type == type).Select(e =>
            {
                return new Options()
                {
                    Id = e.Id,
                    Type = (OptionTypes)e.Type,
                    Value = e.Value,
                    ParentId = e.Parent,
                    Parent = e.Parent.HasValue ? GetOptionById(e.Parent.Value) : null,
                    FaIcon = e.FaIcon
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                list = list.Where(e => e.Value.ToLower().Contains(Search.ToLower())).ToList();
            }

            return list;
        }

        public List<Options> GetOptionsByType(OptionTypes type)
        {
            var list = ampEntities.TBL_Options.ToList().Where(e => (OptionTypes)e.Type == type).Select(e =>
            {
                return new Options()
                {
                    Id = e.Id,
                    Type = (OptionTypes)e.Type,
                    Value = e.Value,
                    ParentId = e.Parent,
                    Parent = e.Parent.HasValue ? GetOptionById(e.Parent.Value) : null,
                    FaIcon = e.FaIcon
                };
            }).ToList();

            return list;
        }

        public List<Options> GetOptionByParentId(int Parent)
        {
            var list = ampEntities.TBL_Options.ToList().Where(e => e.Parent == Parent).Select(e =>
            {
                return new Options()
                {
                    Id = e.Id,
                    Type = (OptionTypes)e.Type,
                    Value = e.Value,
                    ParentId = e.Parent,
                    Parent = e.Parent.HasValue ? GetOptionById(e.Parent.Value) : null,
                    FaIcon = e.FaIcon
                };
            }).ToList();

            return list;
        }

        public Options GetOptionById(int id)
        {
            return ampEntities.TBL_Options.ToList().Where(e => e.Id == id).Select(e =>
            {
                return new Options()
                {
                    Id = e.Id,
                    Type = (OptionTypes)e.Type,
                    Value = e.Value,
                    ParentId = e.Parent,
                    Parent = e.Parent.HasValue ? GetOptionById(e.Parent.Value) : null,
                    FaIcon = e.FaIcon
                };
            }).FirstOrDefault();
        }

        public DbStatus CreateOrUpdateOption(Options model)
        {
            DbStatus status = new DbStatus();
            try
            {
                var record = ampEntities.TBL_Options.Where(e => e.Id == model.Id).FirstOrDefault();
                if (record != null)
                {
                    record.Value = model.Value;
                    record.Type = (int)model.Type;
                    record.Parent = model.ParentId;
                    record.UpdatedOn = DateTime.Now;
                    record.UpdatedBy = model.User.UserId;
                    record.FaIcon = model.FaIcon;

                }
                else
                {
                    var typeExists = ampEntities.TBL_Options.Where(e => e.Value.ToLower().Contains(model.Value.ToLower())
                    && e.Type == (int)model.Type).FirstOrDefault();
                    if (typeExists == null)
                    {
                        record = new TBL_Options()
                        {
                            Value = model.Value,
                            Parent = model.ParentId,
                            Type = (int)model.Type,
                            AddedBy = model.User.UserId,
                            AddedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            UpdatedBy = model.User.UserId,
                            FaIcon = model.FaIcon
                        };
                        ampEntities.TBL_Options.Add(record);
                    }
                    else
                    {
                        status.Message = StringMapperService.GetValue("DuplicateOptionTypeError");
                        status.Status = false;
                        return status;
                    }

                }
                ampEntities.SaveChanges();
                status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                status.Status = true;
            }
            catch (Exception ex)
            {
                status.Message = StringMapperService.GetValue("GeneralUpdateError");
                status.Status = false;
                InsertLog(new LogsModel { CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"), Message = status.Message, FullMessage = ex.Message, ServiceName = "CreateOrUpdateOption" });
            }
            return status;
        }

        public DbStatus DeleteOption(int Id)
        {
            DbStatus status = new DbStatus();
            var record = ampEntities.TBL_Options.Where(e => e.Id == Id).FirstOrDefault();
            if (record != null)
            {
                try
                {
                    ampEntities.TBL_Options.Remove(record);
                    ampEntities.SaveChanges();
                    status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                    status.Status = true;
                }
                catch (Exception ex)
                {
                    status.Message = StringMapperService.GetValue("GeneralUpdateError");
                    status.Status = false;
                    InsertLog(new LogsModel { CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"), Message = status.Message, FullMessage = ex.Message, ServiceName = "DeleteOption" });
                }
            }
            else
            {
                status.Message = StringMapperService.GetValue("DataNotFoundError");
                status.Status = false;
            }
            return status;
        }



        #endregion

        #region logs
        public List<LogsModel> GetAllLogs(string Search = "")
        {
            var list = ampEntities.TBL_Logs.ToList().Select(e =>
            {
                return new LogsModel()
                {
                    Id = e.Id,
                    FullMessage = e.FullMessage,
                    Message = e.Message,
                    ServiceName = e.ServiceName,
                    CreatedOn = e.CreatedOn.ToString("MM/dd/yyyy HH:mm:ss")
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                list = list.Where(e => e.Message.ToLower().Contains(Search.ToLower()) || e.ServiceName.ToLower().Contains(Search.ToLower())).ToList();
            }
            return list;
        }
        public void InsertLog(LogsModel model)
        {
            var record = new TBL_Logs()
            {
                CreatedOn = DateTime.Now,
                FullMessage = model.FullMessage,
                Message = model.Message,
                ServiceName = model.ServiceName
            };
            ampEntities.TBL_Logs.Add(record);
            ampEntities.SaveChanges();

        }
        #endregion

        #region Users
        public List<UsersModel> GetAllUsers(string Search)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var users = entity.TBL_Users.ToList();
                if (!string.IsNullOrWhiteSpace(Search))
                {
                    users = users.Where(e => e.Username.ToLower().Equals(Search.ToLower())).ToList();
                }

                var model = users.Select(e =>
                {
                    return new UsersModel()
                    {
                        Username = e.Username,
                        Department = e.Department,
                        DisplayName = e.DisplayName,
                        Email = e.EmailAddress,
                        EmployeeNo = e.EmployeeNo,
                        Id = e.Id
                    };
                }).ToList();

                return model;
            }
        }

        public UsersModel GetUserId(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var model = entity.TBL_Users.ToList().Where(e => e.Id == id).Select(e =>
                {
                    return new UsersModel()
                    {
                        Username = e.Username,
                        Department = e.Department,
                        DisplayName = e.DisplayName,
                        Email = e.EmailAddress,
                        EmployeeNo = e.EmployeeNo,
                        Id = e.Id,
                        Roles = e.TBL_UserRoleMap.Select(x => x.RoleId.ToString()).ToArray(),
                        AddressLine1 = e.TBL_Contacts.AddressLine1,
                        AddressLine2 = e.TBL_Contacts.AddressLine2,
                        City = e.TBL_Contacts.City,
                        ContactId = e.TBL_Contacts.Id,
                        ContactType = e.TBL_Contacts.ContactTypeId.HasValue ? e.TBL_Contacts.TBL_ContactTypes.Name : "",
                        CountryName = e.TBL_Contacts.CountryId.HasValue ? e.TBL_Contacts.TBL_Country.Name : "",
                        ContactTypeId = e.TBL_Contacts.ContactTypeId,
                        CountryId = e.TBL_Contacts.CountryId,
                        Designation = e.TBL_Contacts.Designation,
                        FaxNumber = e.TBL_Contacts.Faxno,
                        Landline = e.TBL_Contacts.Landline,
                        MobileNumber = e.TBL_Contacts.MobileNumber,
                        Name = e.TBL_Contacts.Name,
                        Organization = e.TBL_Contacts.Organization,
                        Pincode = e.TBL_Contacts.PinCode,
                        ContactImage = e.TBL_Contacts.ContactImg
                    };
                }).FirstOrDefault();

                return model;
            }
        }

        public void CreateOrUpdateUser(UsersModel model)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var record = entity.TBL_Users.ToList().Where(e => (e.Id == model.Id) || (e.Username == model.Username)).FirstOrDefault();
                if (record != null)
                {
                    record.Username = model.Username;
                    record.Department = model.Department;
                    record.DisplayName = model.DisplayName;
                    record.EmailAddress = model.Email;
                    record.EmployeeNo = model.EmployeeNo;
                    record.TBL_Contacts.AddressLine1 = model.AddressLine1;
                    record.TBL_Contacts.AddressLine2 = model.AddressLine2;
                    record.TBL_Contacts.City = model.City;
                    record.TBL_Contacts.ContactTypeId = model.ContactTypeId;
                    record.TBL_Contacts.CountryId = model.CountryId;
                    record.TBL_Contacts.Designation = model.Designation;
                    record.TBL_Contacts.Faxno = model.FaxNumber;
                    record.TBL_Contacts.Landline = model.Landline;
                    record.TBL_Contacts.MobileNumber = model.MobileNumber;
                    record.TBL_Contacts.Name = model.Name;
                    record.TBL_Contacts.Organization = model.Organization;
                    record.TBL_Contacts.PinCode = model.Pincode;
                    record.TBL_Contacts.ContactImg = model.ContactImage;

                    var rolesToRemove = (
                        from u in record.TBL_UserRoleMap
                        join r in (model.Roles ?? new string[0]) on u.RoleId equals int.Parse(r) into rt
                        from rk in rt.DefaultIfEmpty()
                        where rk == null
                        select u
                    ).ToList();

                    foreach (var r in rolesToRemove)
                    {
                        entity.TBL_UserRoleMap.Remove(r);
                    }

                    foreach (var role in model.Roles ?? new string[0])
                    {
                        if (!record.TBL_UserRoleMap.Any(e => e.RoleId == int.Parse(role)))
                        {
                            record.TBL_UserRoleMap.Add(new TBL_UserRoleMap()
                            {
                                RoleId = int.Parse(role),
                                UserId = record.Id
                            });
                        }

                    }
                }
                else
                {
                    var user = new TBL_Users()
                    {
                        Department = model.Department,
                        EmailAddress = model.Email,
                        DisplayName = model.DisplayName,
                        Username = model.Username,
                        EmployeeNo = model.EmployeeNo,
                        TBL_Contacts = new TBL_Contacts()
                        {
                            AddressLine1 = model.AddressLine1,
                            AddressLine2 = model.AddressLine2,
                            City = model.City,
                            Designation = model.Designation,
                            Landline = model.Landline,
                            MobileNumber = model.MobileNumber,
                            Name = model.Name,
                            Organization = model.Organization,
                            PinCode = model.Pincode,
                            Email = model.Email,
                            Faxno = model.FaxNumber,
                            ContactImg = model.ContactImage
                        }

                    };

                    foreach (var role in model.Roles ?? new string[0])
                    {
                        user.TBL_UserRoleMap.Add(new TBL_UserRoleMap()
                        {
                            RoleId = int.Parse(role),
                        });
                    }
                    entity.TBL_Users.Add(user);
                }
                entity.SaveChanges();
            }
        }

        //public void CreateOrUpdateUser(UsersModel model)
        //{
        //    using (AMPEntities entity = new AMPEntities())
        //    {
        //        var record = entity.TBL_Users.ToList().Where(e => (e.Id == model.Id) || (e.Username == model.Username)).FirstOrDefault();
        //        if (record != null)
        //        {
        //            record.Username = model.Username;
        //            record.Department = model.Department;
        //            record.DisplayName = model.DisplayName;
        //            record.EmailAddress = model.Email;
        //            record.EmployeeNo = model.EmployeeNo;
        //            record.TBL_Contacts.AddressLine1 = model.AddressLine1;
        //            record.TBL_Contacts.AddressLine2 = model.AddressLine2;
        //            record.TBL_Contacts.City = model.City;
        //            record.TBL_Contacts.ContactTypeId = model.ContactTypeId;
        //            record.TBL_Contacts.CountryId = model.CountryId;
        //            record.TBL_Contacts.Designation = model.Designation;
        //            record.TBL_Contacts.Faxno = model.FaxNumber;
        //            record.TBL_Contacts.Landline = model.Landline;
        //            record.TBL_Contacts.MobileNumber = model.MobileNumber;
        //            record.TBL_Contacts.Name = model.Name;
        //            record.TBL_Contacts.Organization = model.Organization;
        //            record.TBL_Contacts.PinCode = model.Pincode;
        //            record.TBL_Contacts.ContactImg = model.ContactImage;

        //            var rolesToRemove = (
        //                from u in record.TBL_UserRoleMap
        //                join r in model.Roles on u.RoleId equals int.Parse(r) into rt
        //                from rk in rt.DefaultIfEmpty()
        //                where rk == null
        //                select u
        //            ).ToList();

        //            foreach (var r in rolesToRemove)
        //            {
        //                record.TBL_UserRoleMap.Remove(r);
        //            }

        //            foreach (var role in model.Roles)
        //            {
        //                if (!record.TBL_UserRoleMap.Any(e => e.RoleId == int.Parse(role)))
        //                {
        //                    record.TBL_UserRoleMap.Add(new TBL_UserRoleMap()
        //                    {
        //                        RoleId = int.Parse(role),
        //                        UserId = record.Id
        //                    });
        //                }

        //            }
        //        }
        //        else
        //        {
        //            var user = new TBL_Users()
        //            {
        //                Department = model.Department,
        //                EmailAddress = model.Email,
        //                DisplayName = model.DisplayName,
        //                Username = model.Username,
        //                EmployeeNo = model.EmployeeNo,
        //                TBL_Contacts = new TBL_Contacts()
        //                {
        //                    AddressLine1 = model.AddressLine1,
        //                    AddressLine2 = model.AddressLine2,
        //                    City = model.City,
        //                    Designation = model.Designation,
        //                    Landline = model.Landline,
        //                    MobileNumber = model.MobileNumber,
        //                    Name = model.Name,
        //                    Organization = model.Organization,
        //                    PinCode = model.Pincode,
        //                    Email = model.Email,
        //                    Faxno = model.FaxNumber,
        //                    ContactImg = model.ContactImage
        //                }

        //            };

        //            foreach (var role in model.Roles)
        //            {
        //                user.TBL_UserRoleMap.Add(new TBL_UserRoleMap()
        //                {
        //                    RoleId = int.Parse(role),
        //                });
        //            }
        //            entity.TBL_Users.Add(user);
        //        }
        //        entity.SaveChanges();
        //    }
        //}

        public void DeleteUser(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var record = ampEntities.TBL_Users.Where(e => e.Id == id).FirstOrDefault();
                if (record != null)
                {
                    ampEntities.TBL_Users.Remove(record);
                }
            }
        }

        public List<SelectListItem> GetRoles()
        {
            using (AMPEntities entity = new AMPEntities())
            {
                return entity.TBL_Roles.ToList().Select(e =>
                {
                    return new SelectListItem()
                    {
                        Text = e.RoleName,
                        Value = e.Id.ToString()
                    };
                }).ToList();
            }
        }
        #endregion

        #region Reporting
        public List<ReportingManager> GetAllReports(string search)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                string sql = @"select SPECIFIC_NAME from information_schema.routines 
                  where routine_type = 'PROCEDURE' and SPECIFIC_NAME like 'report_%'";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += string.Format("and SPECIFIC_NAME like '%s'", search);
                }
                var procs = entity.Database.SqlQuery<string>(sql)
                   .Select(x =>
                   {
                       return new ReportingManager()
                       {
                           Name = x.Replace("report_", "").Replace("_", " "),
                           ProcedureName = x
                       };
                   }).ToList();

                return procs;
            }

        }

        public List<ReportingManager> GetAllStandardReports(string search)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                string sql = @"select SPECIFIC_NAME from information_schema.routines 
                  where routine_type = 'PROCEDURE' and SPECIFIC_NAME like 'standard_report_%'";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    sql += string.Format("and SPECIFIC_NAME like '%{0}%'", search);
                }
                var procs = entity.Database.SqlQuery<string>(sql)
                   .Select(x =>
                   {
                       return new ReportingManager()
                       {
                           Name = x.Replace("report_", "").Replace("_", " ").Replace("standard ",""),
                           ProcedureName = x
                       };
                   }).ToList();

                return procs;
            }

        }

        public DataSet RunExcelReport(ReportViewer model)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ARIESConnectionString"].ConnectionString;
            DataSet set = new DataSet();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand sqlComm = new SqlCommand(model.ProcedureName, conn);
                if (model.Filters != null)
                {
                    foreach (var filter in model.Filters)
                    {
                        if (filter.FieldType.ToLower() == "datetime")
                        {
                            sqlComm.Parameters.AddWithValue(filter.FilterName, !string.IsNullOrWhiteSpace(filter.FieldValue) ? DateTimeHelper.ToDateObject(filter.FieldValue) : null);
                        }
                        else if (filter.FieldType.ToLower() == "float")
                        {
                            if (!string.IsNullOrWhiteSpace(filter.FieldValue))
                            {
                                float n;
                                var test = float.TryParse(filter.FieldValue, out n);
                                sqlComm.Parameters.AddWithValue(filter.FilterName, n);
                            }
                            else
                                sqlComm.Parameters.AddWithValue(filter.FilterName, null);

                        }
                        else if (!string.IsNullOrWhiteSpace(filter.FieldValue))
                        {
                            sqlComm.Parameters.AddWithValue(filter.FilterName, filter.FieldValue);
                        }
                        else if (!filter.isNullable && (!filter.hasDefault) && string.IsNullOrWhiteSpace(filter.FieldValue))
                        {
                            sqlComm.Parameters.AddWithValue(filter.FilterName, "");
                        }
                    }
                }
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(set);
            }

            return set;
        }

        public ReportViewer RunReport(ReportViewer model, int pageNo = 1, int pageSize = 10)
        {
            if (model.Filters == null)
            {
                var filters = new List<ViewModels.Dashboad2.Filter>();
                using (AMPEntities entity = new AMPEntities())
                {
                    //p.is_nullable isNullable
                    string sql = string.Format(@"select p.name FilterName,t.name FieldType,has_default_value hasDefault,'' FieldValue, parameter_id Sequence from sys.parameters P
                        inner join sys.types T on P.system_type_id = T.system_type_id
                        where object_id = object_id('dbo.{0}')", model.ProcedureName);
                    filters = entity.Database.SqlQuery<ViewModels.Dashboad2.Filter>(sql).ToList();
                }
                model.Filters = filters.OrderBy(e => e.Sequence).ToList();
            }


            string connStr = ConfigurationManager.ConnectionStrings["ARIESConnectionString"].ConnectionString;
            DataSet set = new DataSet();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand sqlComm = new SqlCommand(model.ProcedureName, conn);
                foreach (var filter in model.Filters)
                {
                    if (filter.FieldType.ToLower() == "datetime")
                    {
                        sqlComm.Parameters.AddWithValue(filter.FilterName, DateTimeHelper.ToDateDbObject(filter.FieldValue));
                    }
                    else if (filter.FieldType.ToLower() == "float")
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FieldValue)) {
                            float n;
                            var test = float.TryParse(filter.FieldValue, out n);
                            sqlComm.Parameters.AddWithValue(filter.FilterName, n);
                        }
                        else
                            sqlComm.Parameters.AddWithValue(filter.FilterName, null);

                    }
                    else if (!string.IsNullOrWhiteSpace(filter.FieldValue))
                    {
                        sqlComm.Parameters.AddWithValue(filter.FilterName, filter.FieldValue);
                    }
                    else if (!filter.isNullable && (!filter.hasDefault) && string.IsNullOrWhiteSpace(filter.FieldValue))
                    {
                        sqlComm.Parameters.AddWithValue(filter.FilterName, "");
                    }
                }
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(set);
            }

            int id = 1;
            foreach (DataTable table in set.Tables)
            {
                var cols = new List<string>();
                foreach (DataColumn col in table.Columns)
                {
                    cols.Add(col.ColumnName);
                }

                List<ReportRow> rows = new List<ReportRow>();
                foreach (DataRow row in table.Rows)
                {
                    List<string> RowData = new List<string>();
                    foreach (var col in cols)
                    {
                        RowData.Add(row[col].ToString());
                    }

                    rows.Add(new ReportRow()
                    {
                        Rows = RowData
                    });
                }

                if (model.Reports.Any(e => e.Id == id))
                {
                    var report = model.Reports.FirstOrDefault(e => e.Id == id);
                    report.Columns = cols;
                    report.Rows.Records = rows;
                }
                else
                {
                    var reportRow = new GridModel<ReportRow>()
                    {

                        Records = rows,
                        PageNo = pageNo,
                        PageSize = pageSize,
                    };
                    model.Reports.Add(new ViewModels.Dashboad2.Report()
                    {
                        Columns = cols,
                        Rows = reportRow,
                        Id = id
                    });
                }
                id++;

            }
            return model;
        }
        
        public void InsertMonthlyFinacleData(DateTime month)
        {
            DbStatus status = new DbStatus();
            string sql = "Execute Insert_Monthly_Status @month";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@month", month));
            status = ExecuteSqlCommand(sql, param);
        }

        #endregion

        #region Activity
        public List<ActivityModel> GetAllActivity(string EntityName, int EntityId)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var records = entity.TBL_Activity.ToList().Where(e => (string.Compare(EntityName, e.EntityName, StringComparison.OrdinalIgnoreCase) == 0) && e.EntityId == EntityId).Select(e =>
                {
                    return new ActivityModel()
                    {
                        Date = e.CreatedOn.HasValue ? e.CreatedOn.Value.ToString("dd", CultureInfo.InvariantCulture) : "",
                        Description = e.Message,
                        Month = e.CreatedOn.HasValue ? e.CreatedOn.Value.ToString("MMMM yy", CultureInfo.InvariantCulture) : "",
                        Name = e.TBL_Users.DisplayName,
                        Id = e.Id,
                        Image = e.TBL_Users.TBL_Contacts.ContactImg != null ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(e.TBL_Users.TBL_Contacts.ContactImg)) : null,
                        LogTime = e.CreatedOn.HasValue ? e.CreatedOn.Value.ToString("HH:mm dd MMM yy", CultureInfo.InvariantCulture) : "",
                    };
                }).OrderByDescending(e => e.Id).Take(5).ToList();
                return records;
            }
        }

        public List<DashboardActivity> GetAllActivity(int limit = 5)
        {
            List<DashboardActivity> activities = new List<DashboardActivity>();
            dynamic records;
            using (AMPEntities entity = new AMPEntities())
            {
                var query = entity.TBL_Activity.OrderByDescending(e => e.Id);

                records = query.Take(limit).Select(e => new
                {
                    Heading = e.Message,
                    UserDisplayName = e.TBL_Users.DisplayName,
                    e.CreatedOn
                }).ToList();
            }
            foreach (var rec in records)
            {
                DashboardActivity activity = new DashboardActivity();
                activity.Heading = rec.Heading;
                try
                {
                    activity.PostScript = string.Format("{0:0.00} hours ago by {1}",
                   (DateTime.Now - rec.CreatedOn).TotalHours,
                   rec.UserDisplayName);
                }
                catch
                {
                    activity.PostScript = string.Format("{0:0.00} hours ago by {1}",
                   (DateTime.Now - (rec.CreatedOn.HasValue ? rec.CreatedOn.Value : DateTime.Now)).TotalHours,
                   rec.UserDisplayName);
                }
                activities.Add(activity);
            }
            return activities;
        }

        public void AddActivity(int recordId, string tableName, string msg)
        {
            using (var entity = new AMPEntities())
            {
                entity.TBL_Activity.Add(new TBL_Activity()
                {
                    CreatedOn = DateTime.Now,
                    EntityId = recordId,
                    EntityName = tableName,
                    Message = msg,
                    CreatedBy = UserConversion.Convertuser().UserId,
                });
                entity.SaveChanges();
            }
        }
        #endregion

        #region Finacle
        public List<Finacle.Models.DisbersmentModel> GetAllDisbursements()
        {
            using (AMPEntities entity = new AMPEntities())
            {
                return entity.Finacle_Disbursement.ToList().Select(e =>
                {
                    return new Finacle.Models.DisbersmentModel
                    {
                        AccountName = e.AccountName,
                        ACID = e.ACID,
                        CurrencyCode = e.CurrencyCode,
                        DisbAmount = e.DisbAmount.HasValue ? e.DisbAmount.Value : 0,
                        DisbSerialNo = e.DisbSerialNo,
                        FORACID = e.FORACID,
                        DisDate = e.DisDate,
                        Id = e.Id,
                        LimitB2KID = e.LimitB2KID,
                        LimitPrefix = e.LimitPrefix,
                        SanctionLimit = e.SanctionLimit.HasValue ? e.SanctionLimit.Value : 0,

                    };
                }).ToList();
            }
        }

        public List<Finacle.Models.RepaymentSchedule> GetAllRepayments()
        {
            using (AMPEntities entity = new AMPEntities())
            {
                return entity.Finacle_RepaymentSchedule.ToList().Select(e =>
                {
                    return new Finacle.Models.RepaymentSchedule
                    {
                        AccountName = e.AccountName,
                        FlowAmount = e.FlowAmount.HasValue ? e.FlowAmount.Value : 0,
                        FORACID = e.FORACID,
                        Currency = e.Currency,
                        FlowStart = e.FlowStart.HasValue ? e.FlowStart.Value : DateTime.Now, //Incorrect
                        LimitPrefix = e.LimitPrefix,
                    };
                }).ToList();
            }
        }
        #endregion

        #region Team
        public List<TeamMappingModel> GetTeam(string EntityName, int EntityId)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var records = entity.TBl_TeamMapping.ToList().Where(e => (string.Compare(EntityName, e.EntityName, StringComparison.OrdinalIgnoreCase) == 0) && e.EntityId == EntityId).Select(e =>
                {
                    return new TeamMappingModel()
                    {
                        Username = e.TBL_Users.Username,
                        Department = e.TBL_Users.Department,
                        DisplayName = e.TBL_Users.DisplayName,
                        Email = e.TBL_Users.EmailAddress,
                        EmployeeNo = e.TBL_Users.EmployeeNo,
                        Id = e.TBL_Users.Id,
                        Roles = e.TBL_Users.TBL_UserRoleMap.Select(x => x.RoleId.ToString()).ToArray(),
                        AddressLine1 = e.TBL_Users.TBL_Contacts.AddressLine1,
                        AddressLine2 = e.TBL_Users.TBL_Contacts.AddressLine2,
                        City = e.TBL_Users.TBL_Contacts.City,
                        ContactId = e.TBL_Users.TBL_Contacts.Id,
                        ContactType = e.TBL_Users.TBL_Contacts.ContactTypeId.HasValue ? e.TBL_Users.TBL_Contacts.TBL_ContactTypes.Name : "",
                        CountryName = e.TBL_Users.TBL_Contacts.CountryId.HasValue ? e.TBL_Users.TBL_Contacts.TBL_Country.Name : "",
                        ContactTypeId = e.TBL_Users.TBL_Contacts.ContactTypeId,
                        CountryId = e.TBL_Users.TBL_Contacts.CountryId,
                        Designation = e.TBL_Users.TBL_Contacts.Designation,
                        FaxNumber = e.TBL_Users.TBL_Contacts.Faxno,
                        Landline = e.TBL_Users.TBL_Contacts.Landline,
                        MobileNumber = e.TBL_Users.TBL_Contacts.MobileNumber,
                        Name = e.TBL_Users.TBL_Contacts.Name,
                        Organization = e.TBL_Users.TBL_Contacts.Organization,
                        Pincode = e.TBL_Users.TBL_Contacts.PinCode,
                        ContactImage = e.TBL_Users.TBL_Contacts.ContactImg,
                        TeamId = e.Id,
                    };
                }).OrderByDescending(e => e.Id).Take(5).ToList();
                return records;
            }
        }

        public void AddTeamMember(int UserId, int EntityId, string EntityName)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                if (!entity.TBl_TeamMapping.ToList().Where(e => (string.Compare(EntityName, e.EntityName, StringComparison.OrdinalIgnoreCase) == 0) && e.EntityId == EntityId).Any(e => e.UserId == UserId))
                {
                    entity.TBl_TeamMapping.Add(new TBl_TeamMapping()
                    {
                        EntityId = EntityId,
                        EntityName = EntityName,
                        UserId = UserId
                    });
                    entity.SaveChanges();
                }
            }
        }

        public void RemoveTeamMember(int id)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var record = entity.TBl_TeamMapping.ToList().FirstOrDefault(e => e.Id == id);
                if (record != null)
                {
                    entity.TBl_TeamMapping.Remove(record);
                }
                entity.SaveChanges();
            }
        }
        #endregion

        #region MapReport
        public List<MapReportModel.Markers> GetLocations(string EntityName, int EntityId)
        {
            using (AMPEntities entity = new AMPEntities())
            {
                var records = entity.TBL_ProjectLocations.Where(p => p.Deleted != 1 && p.SequenceNumber == 1).ToList().Select(e =>
                {
                    return new MapReportModel.Markers()
                    {
                        lat = e.Latitude,
                        lon = e.Longitude,
                        id = e.ProjectId.ToString(),
                        info1 = e.TBL_Projects.Name,
                        info2 = e.TBL_Projects.Status.HasValue ? e.TBL_Projects.TBL_Status.Name : "",
                        info3 = string.Format("$ {0:n0}", e.TBL_Projects.ProjectValue),
                        sector = e.TBL_Projects.Sector,
                    };
                }).ToList();
                return records;
            }
        }
        #endregion

        #region Synclog
        public List<SyncModel> GetSyncLog(string Search = "")
        {
            var list = ampEntities.TBL_SyncLog.ToList().OrderByDescending(e => e.CreatedOn).Select(e =>
            {
                return new SyncModel()
                {
                    Id = e.Id,
                    System = e.System,
                    FullMessage = e.FullMessage,
                    Status = e.Status,
                    ServiceName = e.ServiceName,
                    CreatedOn = e.CreatedOn.ToString("MM/dd/yyyy HH:mm:ss")
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                list = list.Where(e => e.Status.ToLower().Contains(Search.ToLower()) || e.ServiceName.ToLower().Contains(Search.ToLower())).Take(50).ToList();
            }
            else
            {
                list = list.Take(50).ToList();
            }
            return list;
        }
        public void InsertSyncLog(SyncModel model)
        {
            var record = new TBL_SyncLog()
            {
                CreatedOn = DateTime.Now,
                System = model.System,
                FullMessage = model.FullMessage,
                Status = model.Status,
                ServiceName = model.ServiceName
            };
            ampEntities.TBL_SyncLog.Add(record);
            ampEntities.SaveChanges();
        }
        #endregion

        #region StringMapper

        public List<StringMapperModel> GetAllStringMappers(string search = "")
        {
            //return ampEntities.Database.SqlQuery<RegionModel>("exec GetRegions").ToList();
            var query = ampEntities.TBL_String_Mapper.ToList().Select(e =>
            {
                return new StringMapperModel()
                {
                    Id = e.Id,
                    Key = e.Key,
                    Value = e.Value
                };
            });
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e => e.Key.ToLower().Contains(search.ToLower()) || e.Value.ToLower().Contains(search.ToLower()));
            }
            var list = query.ToList();
            return list;

        }

        public DbStatus CreateOrUpdateStringMapper(StringMapperModel model)
        {
            DbStatus status = new DbStatus();
            if (!model.Key.ToString().Contains(" "))
            {
                try
                {
                    var stringmapper = ampEntities.TBL_String_Mapper.Where(e => e.Id == model.Id).FirstOrDefault();
                    if (stringmapper != null)
                    {
                        stringmapper.Key = model.Key;
                        stringmapper.Value = model.Value;
                        status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                        status.Status = true;
                    }
                    else
                    {
                        if (ampEntities.TBL_String_Mapper.Any(e => e.Key.ToLower() == model.Key.ToLower()))
                        {
                            status.Message = StringMapperService.GetValue("DuplicateKeyError");
                            status.Status = false;
                        }
                        else
                        {
                            stringmapper = new TBL_String_Mapper()
                            {
                                Key = model.Key,
                                Value = model.Value
                            };
                            ampEntities.TBL_String_Mapper.Add(stringmapper);
                            status.Message = StringMapperService.GetValue("GeneralUpdateSuccess");
                            status.Status = true;
                        }
                    }
                    ampEntities.SaveChanges();
                }
                catch (Exception ex)
                {
                    status.Message = StringMapperService.GetValue("GeneralUpdateError");
                    status.Status = false;
                    InsertLog(new LogsModel { CreatedOn = DateTime.Now.ToString("dd/MM/yyyy"), Message = status.Message, FullMessage = ex.Message, ServiceName = "CreateOrUpdateStringMapper" });
                }
            }
            else
            {
                status.Message = StringMapperService.GetValue("KeyCannotHaveSpaceError");
                status.Status = false;
            }
            return status;
        }



        #endregion


        #region MailDB
        public static void SaveMailToDB(string to, string CC, string BCC, string mail)
        {
            DbStatus status = new DbStatus();
            try
            {
                using (AMPEntities entity = new AMPEntities())
                {
                    entity.TBL_MailBody.Add(new TBL_MailBody()
                    {
                        to_address = to,
                        cc = CC,
                        bcc = BCC,
                        mail_body = mail,
                        status = "Pending"

                    });
                    entity.SaveChanges();
                    status.Message = "Success";
                    status.Status = true;

                }

            }
            catch (Exception ex)
            {
                status.Message = ex.ToString();
                status.Status = false;

            }
        }

        public List<MailDB> FetchPendingMails()
        {
            var list = ampEntities.TBL_MailBody.ToList().Where(e => e.status.Equals("Pending")).Select(e =>
             {
                 return new MailDB()
                 {
                     Id = e.id,
                     Body = e.mail_body,
                     MailSubject = e.MailSubject,
                     ToAddress = e.to_address,
                     CC = e.cc,
                     BCC = e.bcc,
                     Status = e.status
                 };
             }).ToList();

            return list;
        }

        public void UpdatePendingMails(int id)
        {
            var mail = ampEntities.TBL_MailBody.ToList().Where(e => e.id == id).FirstOrDefault();
            if (mail != null)
            {
                mail.mail_sent_date = DateTime.Now;
                mail.status = "Sent";
            }
            ampEntities.SaveChanges();

        }
        #endregion

    }
}