using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.OData.Json;



namespace DispatchBalanceAPI.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    [ODataRoutePrefix("balance")]
    public class DispatchBalanceController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;
        private readonly DispatchBalanceContext _logcontext;
        private ILogger<DispatchBalanceController> _logger;

        public DispatchBalanceController(DispatchBalanceContext context, ILogger<DispatchBalanceController> logger)
        {
            _context = context;
            _logger = logger;
        }
      

        [ODataRoute("getDispatchBalance")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("getBalance")]
        public async Task<ActionResult<DispatchBalanceFull>> Get(int ceveCode, DateOnly saleDate)
        {
            
            DispatchBalanceHeader DispatchBalanceH = new DispatchBalanceHeader();
            List<DispatchBalanceItem> DispatchBalanceI = new List<DispatchBalanceItem>();
            DispatchBalanceFull DispatchBalance = new DispatchBalanceFull();
            DispatchBalanceFooter DispatchBalanceF = new DispatchBalanceFooter();
            //DispatchBalance = await _context.dbHeader.FirstOrDefault(i => i.SaleDate == saleDate && i.CeveCode == ceveCode);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            DispatchBalanceH = await (from e in _context.dbHeader
                   where e.SaleDate == saleDate && e.CeveCode == ceveCode
                   select e).FirstOrDefaultAsync();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            DispatchBalanceH.ReceivedAsn = await _context.dbAsn.Where(e => e.SaleDate == saleDate && e.CeveCode == ceveCode).Select(e => e.ReferenceNumber).ToArrayAsync();

            DispatchBalanceI = await _context.dbDetail.Where(e => e.SaleDate == saleDate && e.CeveCode == ceveCode).ToListAsync();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            DispatchBalanceF = await _context.dbTotal.FirstOrDefaultAsync(i => i.SaleDate == saleDate && i.CeveCode == ceveCode);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (DispatchBalanceH == null)
            {
               return new DispatchBalanceFull();
           }
     
           DispatchBalance.header = DispatchBalanceH;
           DispatchBalance.items = DispatchBalanceI;
           DispatchBalance.total = DispatchBalanceF;
           var jsonBalance = JsonConvert.SerializeObject(DispatchBalance);
            
            if (jsonBalance == null)
                {
                    return NoContent();
                }
                
            return Ok(jsonBalance);
        }

    }
}
