
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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DispatchBalanceAPI.Controllers
{
    [ApiController, Route("api/v1/[controller]")]
    public class DateNonworkingController : ControllerBase
    {
        private readonly DispatchBalanceContext _context;

        public DateNonworkingController(DispatchBalanceContext context)
        {
            _context = context;
        }

        // GET: api/DatesNoworking
        [ODataRoute("getDatesNoworking")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("getDatesNoworking")]
        public async Task<ActionResult<DatesNonworking>> GetDatesNoworking(string countryCode, string organizationCode, string ceveCode)
        {
            List<DatesNonworking> datesNoworking = new List<DatesNonworking>();
            
            datesNoworking = await (from e in _context.dbDatesNonworking
                                    where e.CountryCode == countryCode && e.OrganizationCode == organizationCode
                                        && e.CeveCode == ceveCode
                                    select e).ToListAsync();

            List<DTO> dates = new List<DTO>();
            DTO date = new DTO();
            for (int i = 0; i < datesNoworking.Count(); i++)            
            {
                date = new DTO()
                {
                    id = i+1,
                    title = "Día No Laboral",
                    start = DateTime.Parse(datesNoworking[i].DateNonworking.ToString()),
                    end = DateTime.Parse(datesNoworking[i].DateNonworking.ToString()).AddDays(1),
                    className = "bg-danger"
                };
                dates.Add(date);
            }
                        
            var jsonDates = JsonConvert.SerializeObject(dates);

            if (dates == null)
            {
                return NotFound();
            }

            return Ok(jsonDates);
        }

        // POST: api/DatesNoworking
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("newDatesNoworking")]
        public async Task<ActionResult<DatesNonworking>> NewDatesNoworking([FromBody] DatesNonworking datesNonworking) 
        {                
            _context.dbDatesNonworking.Add(datesNonworking);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DatesNoworkingExists(datesNonworking.DateNonworking))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(datesNonworking);
        }

        private bool DatesNoworkingExists(string dateNonworking)
        {
            return _context.dbDatesNonworking.Any(e => e.DateNonworking == dateNonworking);
        }


        // DELETE: api/DatesNoworking/5
        [HttpDelete("deleteDatesNoworking")]
        public async Task<IActionResult> DeleteDatesNoworking(string countryCode, string organizationCode, string ceveCode, string dateNonworking)
        {
            var dateNonworkingData = await _context.dbDatesNonworking.FindAsync(countryCode, organizationCode, ceveCode, dateNonworking);
            if (dateNonworkingData == null)
            {
                return NotFound();
            }

            _context.dbDatesNonworking.Remove(dateNonworkingData);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

public class DTO
{
    public int id { get; set; }
    public string title { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public string className { get; set; }
}

