using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models.ApiModels;
namespace AMP.Services
{
    public class AppSimulatorService
    {

        public static List<Contract> GetAllContracts()
        {
            List<Contract> contracts = new List<Contract>();
            contracts.Add(new Contract
            {
                Id = 1,
                ContractCIF = "CDI13901001",
                ContractValue = "HO00001585",
                ContractType = "CC",
                PQNumber = "PQ10001"
            });
            contracts.Add(new Contract
            {
                Id = 2,
                ContractCIF = "CDI13901002",
                ContractValue = "HO00001586",
                ContractType = "CC",
                PQNumber = "PQ10001"
            });
            contracts.Add(new Contract
            {
                Id = 3,
                ContractCIF = "CDI13901003",
                ContractValue = "HO00001589",
                ContractType = "CQ",
                PQNumber = "PQ10003"
            });
            contracts.Add(new Contract
            {
                Id = 4,
                ContractCIF = "CDI13901004",
                ContractValue = "HO00001588",
                ContractType = "CA",
                PQNumber = "PQ10001"
            });
            contracts.Add(new Contract
            {
                Id = 5,
                ContractCIF = "CDI13901005",
                ContractValue = "HO00001585",
                ContractType = "CB",
                PQNumber = "PQ10003"
            });
            contracts.Add(new Contract
            {
                Id = 6,
                ContractCIF = "CDI13901006",
                ContractValue = "HO00001587",
                ContractType = "CA",
                PQNumber = "PQ10001"
            });
            return contracts;
        }

        public static List<Contract> SearchContracts(string pqNumber)
        {
            var list = GetAllContracts();
            list = list.Where(m => m.PQNumber == pqNumber).ToList();
            return list;
        }
    }
}