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
using System.Transactions;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DispatchBalanceAPI.Controllers;

[ApiController, Route("api/v1/[controller]")]
public class MirrorController : ControllerBase
{

    private readonly DispatchBalanceContext _context;

    public MirrorController(DispatchBalanceContext context)
    {
        _context = context;
    }

    [ODataRoute("getMirrorList")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    [HttpGet("getMirrorList")]
    public async Task<ActionResult<MirrorList>> GetMirrorList(DateOnly saleDate, int ceveCode)
    {
    

        List<MirrorList.MirrorListFinalGroup> results = new List<MirrorList.MirrorListFinalGroup>();
        results = await (from s in _context.dbServiceDetail
                         join p in _context.dbProductsList on s.ItemCode equals Convert.ToInt32(p.Internal_codes) into productGroup
                         from product in productGroup.DefaultIfEmpty()
            where (s.TransactionCode == "STA_REC_16" || s.TransactionCode == "STA_REC_17")
               && (s.CeveCode == ceveCode)
               && (s.SaleDate == saleDate)
            orderby s.ItemCode
            select new MirrorList.MirrorListFinalGroup
            {
                ItemCode = s.ItemCode,
                Nombre = ((s.ItemCode.ToString().Length > 4) && (s.ItemCode.ToString().StartsWith("99")))
                            ? _context.dbProductsList
                                      .Where(pl => pl.Internal_codes == s.ItemCode.ToString().Substring(2))
                                      .Select(pl => pl.Large_name)
                                      .FirstOrDefault() ?? ""
                            : product.Large_name,
                Entrada = (s.TransactionCode == "STA_REC_17") ? s.Quantity : 0,
                Salida = (s.TransactionCode == "STA_REC_16") ? s.Quantity : 0
            }).ToListAsync();

        var jsonMovements = JsonConvert.SerializeObject(results);
        if (results == null)
        {
            return NotFound();
        }

        return Ok(jsonMovements);
    }

}
