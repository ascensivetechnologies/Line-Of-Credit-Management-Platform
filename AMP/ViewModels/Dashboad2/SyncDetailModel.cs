using AMP.Models;
using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMP.ViewModels.Dashboad2
{
    public class SyncDetailModel
    {
        public string FinacleLastDate
        {
            get
            {
                return (new Dashboard2ServiceLayer().GetSyncLog("FinacleSyncSuccess").OrderByDescending(e=>e.CreatedOn).FirstOrDefault() ?? new SyncModel()).CreatedOn;
            }
        }
        public string BobLastDate
        {
            get
            {
                return (new Dashboard2ServiceLayer().GetSyncLog("BobSyncSuccess").OrderByDescending(e => e.CreatedOn).FirstOrDefault() ?? new SyncModel()).CreatedOn;
            }
        }
        public List<SyncModel> SyncLog { get
            {
                return new Dashboard2ServiceLayer().GetSyncLog("").OrderByDescending(e => e.CreatedOn).Take(50).ToList();
            }
        }        
    }   
}