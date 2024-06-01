
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DispatchBalanceAPI.Controllers;

[ApiController, Route("api/v1/[controller]")]
public class InventoryConciliationController : ControllerBase
{
    private readonly DispatchBalanceContext _context;

    public InventoryConciliationController(DispatchBalanceContext context)
    {
        _context = context;
    }

    [ODataRoute("getInventoryConciliationList")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    [HttpGet("getInventoryConciliationList")]
    public async Task<ActionResult<InventoryConciliationList>> GetInventoryConciliationList(DateOnly saleDate, int ceveCode)
    {
        //select p.Internal_codes, p.Large_name, 
	    //    s.TransactionCode, s.Quantity as Inventario_Servicios, 
	    //    ISNULL(d.InitialInventory, 0) as Inventario_Corte
        //from ProductsList p
        //left join ServiceDetail s
        //    on s.ItemCode = p.Internal_codes
        //left join DispatchBalanceItem d
        //    on s.CeveCode = d.CeveCode and s.SaleDate = d.SaleDate and d.ItemCode = s.ItemCode
        //WHERE(s.SaleDate = '2023-10-05' or s.SaleDate IS NULL)
        //      and s.CeveCode = '20702'

        List<InventoryConciliationList.InventoryConciliationListFinalGroup> results = new List<InventoryConciliationList.InventoryConciliationListFinalGroup>();
        results = await (from p in _context.dbProductsList
                         join s in _context.dbServiceDetail on p.Internal_codes equals s.ItemCode.ToString() into serviceDetails
                         from sd in serviceDetails.DefaultIfEmpty()
                         join d in _context.dbDetail on new { sd.CeveCode, sd.SaleDate, sd.ItemCode } equals new { d.CeveCode, d.SaleDate, d.ItemCode } into dispatchBalanceItems
                         from dbi in dispatchBalanceItems.DefaultIfEmpty()
                         where (sd.SaleDate == saleDate || sd.SaleDate == null) 
                            && (sd.CeveCode == ceveCode)
                         select new InventoryConciliationList.InventoryConciliationListFinalGroup
                         {
                             Internal_codes = p.Internal_codes,
                             Large_name = p.Large_name,
                             TransactionCode = sd.TransactionCode,
                             Inventario_Servicios = sd.Quantity,
                             Inventario_Corte = (dbi != null) ? dbi.InitialInventory : 0
                         }).ToListAsync();

        var jsonMovements = JsonConvert.SerializeObject(results);
        if (results == null)
        {
            return NotFound();
        }

        return Ok(jsonMovements);
    }
    //consulta para el service code y regresar una lista de transaccion code 
}

