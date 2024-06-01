using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.OData.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Core;
using System.Net.Http;
using NuGet.Protocol.Plugins;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.IdentityModel.Tokens;


//nota se va modificar el dia lunes correspondiente a lo que vamos a realizar 
namespace DispatchBalanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ServiceFunctionController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;
        private readonly HttpClient _httpClient;

        public ServiceFunctionController(DispatchBalanceContext context)
        {
            _context = context;
        }

        [ODataRoute("ServiceFunctionGetASN")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("ServiceFunctionGetASN")]

        public async Task<ActionResult<ServiceSalesDate>> GetRouteDistribution(string ServiceCode, int CeveCode, DateOnly SaleDate)
        {
            var AsnData = await (from r in _context.dbServicesDate
                                   where r.ServiceCode == ServiceCode && r.CeveCode == CeveCode && r.SaleDate == SaleDate
                                   select new ServiceSalesDate
                                   {
                                       ServiceCode = ServiceCode,
                                       CeveCode = r.CeveCode,
                                       SaleDate = r.SaleDate
                                   }).FirstOrDefaultAsync();
            if (AsnData == null)
            {
                await FillTableASN(ServiceCode, CeveCode, SaleDate);

                //Search the data in the table again
                AsnData = await (from s in _context.dbServicesDate
                                   where s.ServiceCode == ServiceCode && s.CeveCode == CeveCode && s.SaleDate == SaleDate
                                   select new ServiceSalesDate
                                   {
                                       ServiceCode = ServiceCode,
                                       CeveCode = s.CeveCode,
                                       SaleDate = s.SaleDate
                                   }).FirstOrDefaultAsync();
            }
            if (RouteData == null)
            {
                return NotFound(new { message = "Not search data in the function of Azure" });

            }
            return Ok(new { message = "All requested data has been successfully retrieved", data = RouteData });
        }
        private async Task FillTableASN(string ServiceCode, int CeveCode, DateOnly SaleDate)
        {
            //Call Azure Function to popular table

            string functionurl = $"https://servicioacceptedasnfuncion.azurewebsites.net/api/opsnapshot/get_acceptedasn_DataService/{SaleDate.ToString("yyyy-MM-dd")}/{CeveCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(functionurl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                //Insert in the base data
                var dataservice = new ServiceSalesDate
                {
                    ServiceCode = ServiceCode,
                    CeveCode = CeveCode,
                    SaleDate = SaleDate
                };
                _context.dbServicesDate.Add(dataservice);
                await _context.SaveChangesAsync();

            }
            else
            {
                //Handle the error
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error in the moment of return the data: {errorMessage}");
            }
        }
    }
}
