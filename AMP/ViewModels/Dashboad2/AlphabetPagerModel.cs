using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Services;
namespace AMP.ViewModels.Dashboad2
{
    public class AlphabetPagerModel
    {
        public string KeyChar { get; set; }
        public string ActiveCssClass { get; set; }

        public static List<AlphabetPagerModel> GetAlphabetPager(string activeKey)
        {
            List<string> alphs = new List<string>();
            Dashboard2ServiceLayer dashboard2Service = new Dashboard2ServiceLayer();
            alphs = dashboard2Service.GetCountryAlphabetLetters();
            var pgr = alphs.Select(m => new AlphabetPagerModel { KeyChar = m, ActiveCssClass = m.ToLower() == activeKey.ToLower() ? "active" : "" }).ToList();
            pgr.Add(new AlphabetPagerModel { KeyChar = "", ActiveCssClass = string.IsNullOrEmpty(activeKey) ? "active" : "" });
            return pgr;
        }
    }
}