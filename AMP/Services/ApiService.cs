using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models.ApiModels;
namespace AMP.Services
{
    public class ApiService
    {
        public static List<Contract> SearchContracts(string pqNumber)
        {
            List<Contract> list = new List<Contract>();
            list = AppSimulatorService.SearchContracts(pqNumber);
            return list;
        }
    }
}