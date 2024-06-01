
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


namespace DispatchBalanceAPI.Controllers
{
    [ApiController, Route("servicelog/api/v1/[controller]")]
    [ODataRoutePrefix("log")]
    public class ServiceLogController: Controller
    {
        private readonly DispatchBalanceContext _context;
        private readonly DispatchBalanceContext _logcontext;
        private ILogger<ServiceLogController> _logger;

        public ServiceLogController(DispatchBalanceContext context, ILogger<ServiceLogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //[ODataRoute("")]
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        //[HttpGet]
        //public async Task<ActionResult<ServiceLogResponse>> FindAll()
        //{
        //    List<ServiceLogRecords> ServiceLogRecords = new List<ServiceLogRecords>();
        //    ServiceLogResponse ServiceLogResponse = new ServiceLogResponse();

        //    ServiceLogRecords = await _context.dbLogDetail.ToListAsync();

        //    ServiceLogResponse.records = ServiceLogRecords;
        //    var jsonBalance = JsonConvert.SerializeObject(ServiceLogResponse);

        //    if (jsonBalance == null)
        //    {
        //        return NoContent();
        //    }

        //    return Ok(jsonBalance);
        //}


        //AG-GRID
        [ODataRoute("getServiceLog")]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet("allServiceLog")]
        public async Task<ActionResult<ServiceLogRecords>> FindAll()
        {
            List<ServiceLogRecords> ServiceLogRecords = new List<ServiceLogRecords>();
            //ServiceLogResponse ServiceLogResponse = new ServiceLogResponse();

            ServiceLogRecords = await _context.dbLogDetail.ToListAsync();

            //ServiceLogResponse.records = ServiceLogRecords;
            var jsonBalance = JsonConvert.SerializeObject(ServiceLogRecords);

            if (jsonBalance == null)
            {
                return NoContent();
            }

            return Ok(jsonBalance);
        }

    }
}
