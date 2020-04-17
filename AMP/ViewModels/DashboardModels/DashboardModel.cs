using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels.DashboardModels
{
    public class DashboardModel
    {
        private AMPEntities ampEntities;
        public List<DashboardLocModel> Locs
        {
            get
            {
                var query = ampEntities.TBL_LOC.AsNoTracking().Select(p => new
                {
                    //RegionId = p.TBL_Country.TBL_Regions.Id,
                    //RegionName = p.TBL_Country.TBL_Regions.Name,
                    p.LocNumber,
                    AmountAllocated = p.TotalAmount,
                    SanctionAmount = ampEntities.Finacle_LocDetails.Where(fl => fl.LimitPrefix.Equals(p.LocNumber))
                    .Select(fl => fl.SanctionAmount.HasValue ? fl.SanctionAmount : 0).FirstOrDefault(),
                    DisbAmt = ampEntities.Finacle_Disbursement.Where(e => e.FORACID.Equals(p.LocAccountNo)).Sum(a => a.DisbAmount)
                })
                    .Select(p1 => new
                    {
                        p1.LocNumber,
                        p1.AmountAllocated,
                        p1.DisbAmt,
                    }).Select(p5 => new DashboardLocModel
                    {
                        Name = p5.LocNumber,
                        Allocated = p5.AmountAllocated.HasValue ? p5.AmountAllocated.Value : 0,
                        Disbursed = p5.DisbAmt.HasValue ? p5.DisbAmt.Value : 0,
                        Percentage = ((p5.AmountAllocated.HasValue ? p5.AmountAllocated.Value : 0) > 0 ? (((p5.DisbAmt.HasValue ? p5.DisbAmt.Value : 0) / p5.AmountAllocated.Value) * 100) : 0),
                    })
                    .OrderByDescending(p5 => p5.Disbursed)
                    .Take(5);
                return query.ToList();
                //return new Dashboard2ServiceLayer().GetAllLocs().GroupBy(e => new { e.Region.Id, e.Region.Name }).Select(e =>
                //  {
                //      float val = 0;
                //      var TotalAmount = e.Sum(x => float.TryParse(x.AmountAllocated, out val) ? val : 0);
                //      var Utilized = e.Sum(x => (x.Utilization/100) * (float.TryParse(x.AmountAllocated, out val) ? val : 0));
                //      return new DashboardRegionModel()
                //      {
                //          Name = e.Key.Name,
                //          Amount = TotalAmount,
                //          Percentage = TotalAmount > 0 ? ((Utilized / TotalAmount) * 100) : 0
                //      };
                //  }).OrderByDescending(e => e.Amount).Take(5).ToList();
            }
        }
        public List<DashboardRegionModel> Regions
        {
            get
            {
                var query = ampEntities.TBL_LOC.AsNoTracking().Select(p => new
                {
                    RegionId = p.TBL_Country.TBL_Regions.Id,
                    RegionName = p.TBL_Country.TBL_Regions.Name,
                    AmountAllocated = p.TotalAmount,
                    SanctionAmount = ampEntities.Finacle_LocDetails.Where(fl => fl.LimitPrefix.Equals(p.LocNumber))
                    .Select(fl => fl.SanctionAmount.HasValue ? fl.SanctionAmount : 0).FirstOrDefault(),
                    DisbAmt = ampEntities.Finacle_Disbursement.Where(e => e.FORACID.Equals(p.LocAccountNo)).Sum(a => a.DisbAmount)
                })
                    .Select(p1 => new
                    {
                        p1.RegionId,
                        p1.RegionName,
                        p1.AmountAllocated,
                        Utilization = p1.SanctionAmount.HasValue ? (p1.DisbAmt / p1.SanctionAmount.Value * 100) : 0,
                    })
                    .GroupBy(p3 => p3.RegionName)
                    .Select(p4 => new
                    {
                        Name = p4.Key,
                        Amount = p4.Sum(amt => amt.AmountAllocated.HasValue ? amt.AmountAllocated : 0),
                        Utilized = p4.Sum(utl => (utl.Utilization / 100) * (utl.AmountAllocated.HasValue ? utl.AmountAllocated.Value : 0)),
                    })
                    .Select(p5 => new DashboardRegionModel
                    {
                        Name = p5.Name,
                        Amount = p5.Amount.HasValue ? p5.Amount.Value : 0,
                        Percentage = ((p5.Amount.HasValue ? p5.Amount.Value : 0) > 0 ? ((p5.Utilized / p5.Amount) * 100) : 0).Value,
                    })
                    .OrderByDescending(p5 => p5.Amount)
                    .Take(5);
                return query.ToList();
                //return new Dashboard2ServiceLayer().GetAllLocs().GroupBy(e => new { e.Region.Id, e.Region.Name }).Select(e =>
                //  {
                //      float val = 0;
                //      var TotalAmount = e.Sum(x => float.TryParse(x.AmountAllocated, out val) ? val : 0);
                //      var Utilized = e.Sum(x => (x.Utilization/100) * (float.TryParse(x.AmountAllocated, out val) ? val : 0));
                //      return new DashboardRegionModel()
                //      {
                //          Name = e.Key.Name,
                //          Amount = TotalAmount,
                //          Percentage = TotalAmount > 0 ? ((Utilized / TotalAmount) * 100) : 0
                //      };
                //  }).OrderByDescending(e => e.Amount).Take(5).ToList();
            }
        }
        public List<DashboardActivity> Activity
        {
            get
            {
                return new Dashboard2ServiceLayer().GetAllActivity(limit: 5);
            }
        }
        public List<DashboardDevelopmentSector> Sectors
        {
            get
            {
                var totalAmount = ampEntities.TBL_Projects.AsNoTracking().Sum(c => (Decimal?)c.ProjectValue) ?? 0;
                //var totalAmount = new Dashboard2ServiceLayer().GetAllProjects(null).Sum(e => e.ProjectValue);
                var result = ampEntities.TBL_Projects.AsNoTracking().GroupBy(p => p.Sector)
                    .Select(e => new DashboardDevelopmentSector
                    {
                        Name = e.Key,
                        Percentage = (e.Sum(x => x.ProjectValue) / totalAmount) * 100,
                    }).OrderByDescending(e => e.Percentage).Take(5).ToList();

                return result;
                //return new Dashboard2ServiceLayer().GetAllProjects(null).GroupBy(e => e.Sector).Select(e =>
                //{
                //    var total = e.Sum(x => x.ProjectValue);
                //    return new DashboardDevelopmentSector
                //    {
                //        Name = e.Key,
                //        Percentage = (total / totalAmount) * 100,
                //    };
                //}).OrderByDescending(e => e.Percentage).Take(5).ToList();
            }
        }
        public List<DashboardMemberCountries> MemberCountries
        {
            get
            {
                var result = ampEntities.TBL_LOC.AsNoTracking().Select(m => new
                {
                    m.TotalAmount,
                    Name = m.TBL_Country.Name
                }).GroupBy(m => m.Name).Select(e => new DashboardMemberCountries
                {
                    Amount = e.Sum(x => x.TotalAmount.HasValue ? x.TotalAmount.Value : 0),
                    Name = e.Key
                }
                    ).OrderByDescending(e => e.Amount).Take(6).ToList();

                return result;
                //return new Dashboard2ServiceLayer().GetAllLocs().GroupBy(e => e.CountryName).Select(e =>
                //{
                //    decimal amount = 0;
                //    return new DashboardMemberCountries()
                //    {
                //        Amount = e.Sum(x => decimal.TryParse(x.AmountAllocated,out amount) ? amount : 0),
                //        Name = e.Key
                //    };
                //}).OrderByDescending(e => e.Amount).Take(6).ToList();
            }
        }
        public List<DisbvRepaymentsModel> Disb
        {
            get
            {
                return ampEntities.Finacle_Disbursement.GroupBy(m => m.DisDate.Value.Year)
                   .Select(m => new DisbvRepaymentsModel
                   {
                       Count = m.Count(),
                       Year = m.Key
                   }).OrderByDescending(m => m.Year).Take(10).ToList();
                //return new Dashboard2ServiceLayer().GetAllDisbursements().GroupBy(e => e.DisDate.HasValue ? e.DisDate.Value.Year : 0).Select(e =>
                //{
                //    return new DisbvRepaymentsModel()
                //    {
                //       Count = e.Count(),
                //       Year = e.Key
                //    };

                //}).OrderByDescending(e => e.Year).Where(e => e.Year != 0).Take(10).ToList();
            }
        }
        public List<DisbvRepaymentsModel> Repayments
        {
            get
            {

                return ampEntities.Finacle_RepaymentSchedule.GroupBy(m => m.FlowStart.Value.Year)
                    .Select(m => new DisbvRepaymentsModel
                    {
                        Count = m.Count(),
                        Year = m.Key
                    }).OrderByDescending(m => m.Year).Take(10).ToList();

                //return new Dashboard2ServiceLayer().GetAllRepayments().GroupBy(e => e.FlowStart.Year).Select(e =>
                //{
                //    return new DisbvRepaymentsModel()
                //    {
                //        Count = e.Count(),
                //        Year = e.Key
                //    };

                //}).OrderByDescending(e => e.Year).Where(e => e.Year != 0).Take(10).ToList();
            }
        }

        public decimal TotalAmount
        {
            get
            {
                decimal? a = 0;
                a = ampEntities.TBL_LOC.AsNoTracking().Sum(e => e.TotalAmount);
                return a.HasValue ? a.Value : 0;
            }
        }

        public int Countries
        {
            get
            {
                var count = ampEntities.TBL_LOC.AsNoTracking().GroupBy(e => e.CountryId).Count();
                return count;
            }
        }

        public DashboardModel()
        {
            ampEntities = new AMPEntities();
        }
    }
}