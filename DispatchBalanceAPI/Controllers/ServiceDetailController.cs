using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DispatchBalanceAPI.Controllers
{

    public enum ServiceType
    {
        RouteUnload,
        AcceptedASN,
        DistributionRoute,
        Incidents
    }

    [Route("api/[controller]")]
    [ApiController]

    public class ServiceDetailController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;
        private readonly Dictionary<string, string> serviceVariables;
        public ServiceDetailController(DispatchBalanceContext context)
        {
            _context = context;


            //Defines a dictionary that maps serviceCode to a variable type
            serviceVariables = new Dictionary<string, string>
        {
            {"RouteUnload" ,"RouteUnload" },
            {"AcceptedASN","AcceptedASN"},
            {"DistributionRoute","DistributionRoute" },
            {"incidents", "incidents"},
            // Añade más mapeos según sea necesario
        };
        }
        [ODataRoute("GetAllServiceDetailUnion")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("GetSaleData")]

        public async Task<ActionResult<IQueryable<ServiceDetail>>> GetSaleData(string ServiceCode,int CeveCode, DateOnly SaleDate)
        {
            if(!serviceVariables.ContainsKey(ServiceCode))
            {
                return BadRequest("invalid service code");
            }

            var variableType = serviceVariables[ServiceCode];

            IQueryable<ServiceDetail> query = null;

            switch (variableType)
            {
                case "RouteUnload":
                    query = GetRouteUnloadData(ServiceCode, CeveCode,SaleDate);
                    break;
                case "AcceptedASN":
                    query = GetAcceptedASNData(ServiceCode, CeveCode, SaleDate);
                    break;
                case "DistributionRoute":
                    query = GetDistributionRouteData(ServiceCode, CeveCode, SaleDate);
                    break;
                case "incidents":
                    query = GetIncidentsData(ServiceCode, CeveCode, SaleDate);
                    break;
                default:
                    return BadRequest("Unrecognized variable type");
            }

            if (query == null)
            {
                return NotFound("No data found for the provided ServiceCode.");
            }

            return Ok(query);
        }

        private IQueryable<ServiceDetail> GetRouteUnloadData(string serviceCode, int CeveCode, DateOnly SaleDate)
        {
            return _context.dbServiceDetail
               .Join(_context.dbInventoryTransactionsCatalog,
                     sd => sd.TransactionCode,  
                     tc => tc.TransactionCode,
                     (sd, tc) => new { sd, tc })
               .Where(x => x.tc.ServiceCode == serviceCode && x.tc.Type == ServiceType.RouteUnload.ToString() && x.sd.CeveCode == CeveCode && x.sd.SaleDate == SaleDate)
               .Select(x => x.sd);
        }

        private IQueryable<ServiceDetail> GetAcceptedASNData(string serviceCode, int CeveCode, DateOnly SaleDate)
        {
            return _context.dbServiceDetail
                .Join(_context.dbInventoryTransactionsCatalog,
                      sd => sd.TransactionCode,
                      tc => tc.TransactionCode,
                      (sd, tc) => new { sd, tc })
                .Where(x => x.tc.ServiceCode == serviceCode && x.tc.Type == ServiceType.AcceptedASN.ToString()&& x.sd.CeveCode == CeveCode &&x.sd.SaleDate==SaleDate)
                .Select(x => x.sd);
        }

        private IQueryable<ServiceDetail> GetDistributionRouteData(string serviceCode, int CeveCode, DateOnly SaleDate)
        {
            return _context.dbServiceDetail
                 .Join(_context.dbInventoryTransactionsCatalog,
                       sd => sd.TransactionCode,
                       tc => tc.TransactionCode,
                       (sd, tc) => new { sd, tc })
                .Where(x => x.tc.ServiceCode == serviceCode && x.tc.Type == ServiceType.DistributionRoute.ToString() && x.sd.CeveCode == CeveCode && x.sd.SaleDate == SaleDate)
                 .Select(x => x.sd);
        }

        private IQueryable<ServiceDetail> GetIncidentsData(string serviceCode, int CeveCode, DateOnly SaleDate)
        {
            return _context.dbServiceDetail
                .Join(_context.dbInventoryTransactionsCatalog,
                      sd => sd.TransactionCode,
                      tc => tc.TransactionCode,
                      (sd, tc) => new { sd, tc })
                .Where(x => x.tc.ServiceCode == serviceCode && x.tc.Type == ServiceType.Incidents.ToString() && x.sd.CeveCode == CeveCode && x.sd.SaleDate == SaleDate)
                .Select(x => x.sd);
        }
    }
}


