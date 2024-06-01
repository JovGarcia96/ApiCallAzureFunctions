
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.OData;
using System.Transactions;
using Newtonsoft.Json;

namespace DispatchBalanceAPI.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class MovementRoutingController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;

        public MovementRoutingController(DispatchBalanceContext context)
        {
            _context = context;
        }
        
        // GET: api/SystemDates/5
        [ODataRoute("getMovementRouting")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        //[HttpGet("getMovementRouting/{saleDate}")]
        [HttpGet("getMovementRouting")]
        public async Task<ActionResult<MovementRouting>> GetMovementRouting(DateOnly saleDate, int ceveCode)
        {
          

            List<MovementRoutingItems> results = new List<MovementRoutingItems>();
            results = await (from i in _context.dbInventoryTransactionsCatalog
                             join s in _context.dbServiceDetail on i.TransactionCode equals s.TransactionCode into joined                              
                             from j in joined.DefaultIfEmpty()
                             group j by new { i.TransactionCode, j.CeveCode, j.SaleDate, i.TransactionDescription, i.OutgoingIncome } into gp
                             where (gp.Key.CeveCode == ceveCode || gp.Key.CeveCode == null) &&
                                   (gp.Key.SaleDate == saleDate || gp.Key.SaleDate == null) 
                             orderby gp.Key.OutgoingIncome
                             select new MovementRoutingItems
                             {
                                 Quantity = gp.Sum(x => x.Quantity),
                                 TransactionCode = gp.Key.TransactionCode,
                                 CeveCode = ceveCode, 
                                 SaleDate = saleDate, 
                                 TransactionDescription = gp.Key.TransactionDescription,
                                 OutgoingIncome = gp.Key.OutgoingIncome
                             }).ToListAsync();


         
            var jsonMovements = JsonConvert.SerializeObject(results);
            if (results == null)
            {
                return NotFound();
            }

            return Ok(jsonMovements);
        }
    }
}
