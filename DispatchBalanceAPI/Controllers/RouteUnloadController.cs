using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.OData.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Core;
using System.Net.Http;
using NuGet.Protocol.Plugins;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DispatchBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteUnloadController : ControllerBase
    {
       private  DispatchBalanceContext _context;
       private  HttpClient _httpClient;

        public RouteUnloadController(DispatchBalanceContext context)
        {
            _context = context;
        }

        [ODataRoute("GetRouteUnload")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("GetRouteDistribution")]

        public async Task<ActionResult<ServiceSalesDate>>GetRouteUnload(string ServiceCode, int CeveCode, DateOnly SaleDate)
        {
            var RouteUnloadData =  await (from u in _context.dbServicesDate
                                          where u.ServiceCode == ServiceCode && u.CeveCode == CeveCode && u.SaleDate == SaleDate select new ServiceSalesDate
                                          {
                                              ServiceCode = ServiceCode,
                                              CeveCode = CeveCode,  
                                              SaleDate = SaleDate
                                          }).FirstOrDefaultAsync();
            if (RouteUnloadData == null)
            {
                await ConstructFillTableServiceDate(ServiceCode, CeveCode, SaleDate);
                // search  the data in the table again
                RouteUnloadData = await (from u in _context.dbServicesDate
                                         where u.ServiceCode == ServiceCode && u.CeveCode == CeveCode && u.SaleDate == SaleDate
                                         select new ServiceSalesDate
                                         {
                                             ServiceCode = ServiceCode,
                                             CeveCode = CeveCode,
                                             SaleDate = SaleDate
                                         }).FirstOrDefaultAsync();
            }
                if (RouteUnloadData == null)
                {
                    return NotFound(new { message = "Not Search data in the function of Azure" });
                }
                return Ok(new { message = "All requested data has been successfully retrieved", data = RouteUnloadData });
            }
            
          
        
        private async Task ConstructFillTableServiceDate(string serviceCode, int CeveCode, DateOnly SaleDate)
        {
            //Call Azure Function to popular table 
            string functionurl = $"https://functionrouteunload.azurewebsites.net/api/opsnapshot/get_RouteUnload_DataService/{SaleDate.ToString("yyyy-MM-dd")}/{CeveCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(functionurl);
            if (response.IsSuccessStatusCode)
            {
                string  responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                //Insert  in the base  data 
                var dateService = new ServiceSalesDate
                {
                    ServiceCode = serviceCode,
                    CeveCode = CeveCode,
                    SaleDate = SaleDate
                };
                _context.dbServicesDate.Add(dateService);
                await _context.SaveChangesAsync();
            }
            else
            {
                //Handle the error 
                string errorMesaage = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorMesaage);    
            }
        }

    }
}
