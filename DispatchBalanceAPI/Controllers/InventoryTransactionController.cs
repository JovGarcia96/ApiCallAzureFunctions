
﻿using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using DispatchBalanceAPI.Model;
using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.OData.Json;
using Microsoft.IdentityModel.Protocols;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DispatchBalanceAPI.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    [ODataRoutePrefix("inventoryTransaction")]
    public class InventoryTransactionsController : Controller
    {
        private readonly DispatchBalanceContext _context;
        private readonly DispatchBalanceContext _logcontext;
        private ILogger<InventoryTransactionsController> _logger;

        public InventoryTransactionsController(DispatchBalanceContext context, ILogger<InventoryTransactionsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        //AG-GRID DESCOMENTAR ESTA FUNCIÓN
        [ODataRoute("getInventoryTransaction")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("allInventoryTransaction")]
        public async Task<ActionResult<InventoryTransactionsCatalog>> FindAll()
        {
            List<InventoryTransactionsCatalog> InventoryTransactionRecords = new List<InventoryTransactionsCatalog>();
            //InventoryTransactionsResponse InventoryTransactionResponse = new InventoryTransactionsResponse();

            //var inventoryTransactions = await _context.dbInventoryTransaction.ToListAsync();
            var inventoryTransactions = await _context.dbInventoryTransactionsCatalog.ToListAsync();
            //InventoryTransactionRecords.records.Add(inventory);
            InventoryTransactionRecords = inventoryTransactions;
            //InventoryTransactionResponse = InventoryTransactionRecords;
            var jsonBalance = JsonConvert.SerializeObject(InventoryTransactionRecords);

            if (jsonBalance == null)
            {
                return NoContent();
            }

            return Ok(jsonBalance);
        }





        // GET: api/InventoryTransactionsRecords/5
        [ODataRoute("getInventoryTransaction")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("oneInventoryTransaction")]
        public async Task<ActionResult<InventoryTransactionsRecords>> FindOne(string transactionCode)
        {
            List<InventoryTransactionsCatalog> InventoryTransactionRecords = new List<InventoryTransactionsCatalog>();

            var inventoryTransaction = await (from e in _context.dbInventoryTransactionsCatalog
            where e.TransactionCode == transactionCode
                                              select e).ToListAsync();

            InventoryTransactionRecords = inventoryTransaction;

            var jsonBalance = JsonConvert.SerializeObject(InventoryTransactionRecords);

            if (jsonBalance == null)
            {
                return NotFound();
            }

            return Ok(jsonBalance);
        }







        // PUT: api/SystemDate/?ceveCode=5&saleDate=2021-08-01
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ODataRoute("editInventoryTransaction")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpPut("editInventoryTransaction")]
        public async Task<IActionResult> EditInventoryTransaction(InventoryTransactionsCatalog record)
        {
            var obj = JsonConvert.DeserializeObject<InventoryTransactionsCatalog>(JsonConvert.SerializeObject(record));

            //Get the product from DB to update
            var product = _context.dbInventoryTransactionsCatalog.SingleOrDefault(x => x.TransactionCode == obj.TransactionCode);

            //update fields...
            product.TransactionDescription = obj.TransactionDescription;
            product.OutgoingIncome = obj.OutgoingIncome;
            product.Type = obj.Type;
            product.ServiceCode = obj.ServiceCode;
            product.CreatedOn = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            product.CreatedBy = "cortedespachoWeb";
            product.ModifiedOn = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            product.ModifiedBy = "cortedespachoWeb";
            //Save changes
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (InventoryTransactionExists(obj.TransactionCode))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //AG-GRID DESCOMENTAR ESTA FUNCIÓN
        // POST: api/InventoryTransaction
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("newInventoryTransaction")]
        public async Task<ActionResult<InventoryTransactionsCatalog>> NewInventoryTransaction(InventoryTransactionsCatalog record)
        {
            //List<InventoryTransactionsCatalog> inventoryTransaction = new List<InventoryTransactionsCatalog>();

            //foreach (var item in record)
            //{
                InventoryTransactionsCatalog r = new InventoryTransactionsCatalog();
                r.TransactionCode = record.TransactionCode;
                r.TransactionDescription = record.TransactionDescription;
                r.OutgoingIncome = record.OutgoingIncome;
                r.Type = record.Type;
                r.ServiceCode = record.ServiceCode;
                r.CreatedOn = record.CreatedOn;
                r.CreatedBy = record.CreatedBy;
                r.ModifiedOn = record.ModifiedOn;
                r.ModifiedBy = record.ModifiedBy;
                _context.dbInventoryTransactionsCatalog.Add(r);
            //}

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InventoryTransactionExists(r.TransactionCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            //return CreatedAtAction("getInventoryTransaction", new { id = inventoryTransacction.TransactionCode }, inventoryTransacction);
            return Ok(r);
        }

        private bool InventoryTransactionExists(string transactionCode)
        {
            return _context.dbInventoryTransactionsCatalog.Any(e => e.TransactionCode == transactionCode);
        }


       
        [HttpDelete("deleteInventoryTransaction")]
        public async Task<IActionResult> DeleteInventoryTransaction(string transactionCode)
        {
            var inventoryData = await _context.dbInventoryTransactionsCatalog.FindAsync(transactionCode);
            if (inventoryData == null)
            {
                return NotFound();
            }

            _context.dbInventoryTransactionsCatalog.Remove(inventoryData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
