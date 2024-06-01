using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.OData.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Core;
using System.Net.Http;

namespace DispatchBalanceAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class ServiceSalesDateController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;
        private readonly HttpClient _httpClient;

        public ServiceSalesDateController(DispatchBalanceContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
        }

        [ODataRoute("getSaleData")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("getSaleData")]
        public async Task<ActionResult<ServiceSalesDate>> GetSaleData(string ServiceCode, int CeveCode, DateOnly SaleDate)
        {
            var saleData = await (from s in _context.dbServicesDate
                                  where s.ServiceCode == ServiceCode && s.CeveCode == CeveCode && s.SaleDate == SaleDate
                                  select new ServiceSalesDate
                                  {
                                      ServiceCode = ServiceCode,
                                      CeveCode = s.CeveCode,
                                      SaleDate = s.SaleDate
                                  }).FirstOrDefaultAsync();

            if (saleData == null)
            {
                await FillTableAsync(ServiceCode, CeveCode, SaleDate);
                //Search the data in the table again
                saleData = await (from s in _context.dbServicesDate
                                  where s.ServiceCode == ServiceCode && s.CeveCode == CeveCode && s.SaleDate == SaleDate
                                  select new ServiceSalesDate
                                  {
                                      ServiceCode = ServiceCode,
                                      CeveCode = s.CeveCode,
                                      SaleDate = s.SaleDate
                                  }).FirstOrDefaultAsync();
            }
            if (saleData == null)
            {
                return NotFound();
            }

            return Ok(new { message = "All requested data has been successfully retrieved", data = saleData });
        }
        private async Task FillTableAsync(string ServiceCode, int CeveCode, DateOnly SaleDate)
        {
            // Call Azure function to populate table

            string functionUrl = $"https://funcioninventorytransaction.azurewebsites.net/api/opsnapshot/get_incidents_DataService/{SaleDate.ToString("yyyy-MM-dd")}/{CeveCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(functionUrl);


            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                //insert in the basedata
                var newSaleData = new ServiceSalesDate
                {
                    ServiceCode = ServiceCode,
                    CeveCode = CeveCode,
                    SaleDate = SaleDate,
                    // Assign other fields according to the data received
                };
                _context.dbServicesDate.Add(newSaleData);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the error
                string errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}



