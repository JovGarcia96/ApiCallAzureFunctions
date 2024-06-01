using System;
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

namespace DispatchBalanceAPI.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class SystemDateController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;

        public SystemDateController(DispatchBalanceContext context)
        {
            _context = context;
        }

        //// GET: api/SystemDates
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<SystemDate>>> GetSystemDate()
        //{
        //    return await _context.dbSystem.ToListAsync();
        //}

        // GET: api/SystemDates/5
        [ODataRoute("getSystemDate")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("getSaleDate")]
        public async Task<ActionResult<SystemDate>> GetSystemDate(int ceveCode)
        {
            var systemDate = await (from e in _context.dbSystem
                                    where e.CeveCode == ceveCode && e.Status == 'A'
                                    select e).FirstOrDefaultAsync();

            if (systemDate == null)
            {
                return NotFound();
            }

            return systemDate;
        }

        // PUT: api/SystemDate/?ceveCode=5&saleDate=2021-08-01
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ODataRoute("setSystemDate")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpPut("putSaleDate")]
        public async Task<IActionResult> PutSystemDate(int ceveCode, DateOnly saleDate)
        {
            //if (ceveCode != ceveCode)
            //{
            //    return BadRequest();
            //}
            SystemDate systemDate = new SystemDate();
            systemDate.CeveCode = ceveCode;
            systemDate.SaleDate = saleDate;
            systemDate.Status = 'C';
            _context.dbSystem.Attach(systemDate);
            _context.Entry(systemDate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemDateExists(ceveCode))
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

        // POST: api/SystemDates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("postSaleDate")]
        public async Task<ActionResult<SystemDate>> PostSystemDate(int ceveCode, DateOnly saleDate)
        {

            SystemDate systemDate = new SystemDate();
            systemDate.CeveCode = ceveCode;
            systemDate.SaleDate = saleDate.AddDays(1);
            systemDate.Status = 'A';
            _context.dbSystem.Add(systemDate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SystemDateExists(systemDate.CeveCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("getSystemDate", new { id = systemDate.CeveCode }, systemDate);
        }

        // DELETE: api/SystemDates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemDate(int id)
        {
            var systemDate = await _context.dbSystem.FindAsync(id);
            if (systemDate == null)
            {
                return NotFound();
            }

            _context.dbSystem.Remove(systemDate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SystemDateExists(int ceveCode)
        {
            return _context.dbSystem.Any(e => e.CeveCode == ceveCode);
        }
    }
}
