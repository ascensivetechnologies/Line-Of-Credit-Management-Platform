using AMP.ViewModels.Dashboad2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Services
{
    public static class GridService
    {
        #region Regions
        public static List<RegionModel> GetRegions(int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {

            var records = new Dashboard2ServiceLayer().GetAllRegions().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Name.ToLower().Contains(searchString.ToLower()) || p.AddedBy.ToLower().Contains(searchString.ToLower()) || p.AddedOn.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            total = records.Count();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            return records.ToList();
        }
        public static void SaveRegion(RegionModel model)
        {

            new Dashboard2ServiceLayer().CreateOrUpdateRegion(model);
            
        }

        public static void RemoveRegion(int id)
        {
            new Dashboard2ServiceLayer().DeleteRegion(id);
        }


        #endregion

        #region Countries
        public static List<CountryModel> GetCountries(int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {

            var records = new Dashboard2ServiceLayer().GetAllCountries().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.CountryName.ToLower().Contains(searchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            total = records.Count();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            return records.ToList();
        }
        public static void SaveCountry(CountryModel model)
        {

            new Dashboard2ServiceLayer().CreateOrUpdateCountry(model);

        }

        public static void RemoveCountry(int id)
        {
            new Dashboard2ServiceLayer().DeleteCountry(id);
        }
        #endregion
    }
}