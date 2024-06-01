
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

namespace DispatchBalanceAPI.Controllers;

[ApiController, Route("api/v1/[controller]")]
public class TransferProductController : ControllerBase
{
    //        select distinct(Internal_codes), sum(Entrada) Entrada, sum(Salida) Salida
    //from
    //    (select Internal_codes, Large_name, Quantity, CeveCode, SaleDate, OutgoingIncome, s.TransactionCode,
    //		case when Quantity > 0 THEN Quantity Else 0 End as Entrada,
    //		case when Quantity < 0 THEN Quantity Else 0 End as Salida
    //    from ProductsList p

    //        left JOIN ServiceDetail s

    //            on s.ItemCode = p.Internal_codes

    //        left join InventoryTransactionsCatalog i

    //            on s.TransactionCode = i.TransactionCode

    //    WHERE (s.SaleDate = '2023-10-05' or s.SaleDate IS NULL)

    //        and s.CeveCode = '20702' and s.TransactionCode in ('STA_RES_19', 'STA_RES_20')

    //    GROUP BY p.Internal_codes, p.Large_name, s.CeveCode, s.SaleDate, s.Quantity, i.OutgoingIncome, s.TransactionCode) as resultado
    //group by Internal_codes

    //--INSERT[dbo].[ServiceDetail] ([TransactionCode], [CeveCode], [SaleDate], [ItemCode], [Quantity], [AuditInfo_CreatedBy], [AuditInfo_CreatedOn], [AuditInfo_ModifiedBy], [AuditInfo_ModifiedOn]) VALUES(N'STA_RES_20', 20702, CAST(N'2023-10-05' AS Date), 123662, 75, N'incident', CAST(N'2024-04-03T14:03:15.1628186' AS DateTime2), N'incident', CAST(N'2024-04-03T14:03:15.1628198' AS DateTime2))

    //--SELECT ItemProduct, ProductName FROM ProductsList p

    //--SELECT ItemCode, Quantity, TransactionCode, CeveCode, SaleDate FROM ServiceDetail where CeveCode = '20702' order by TRansactionCode, ItemCode;

    //--UPDATE ServiceDetail
    //--SET TransactionCode = 'STA_RES_20'
    //--WHERE CeveCode = '20702' and TransactionCode = 'PR_RES_05' and ItemCode = 123662;

    //--select* from InventoryTransactionsCatalog

    //select* from ServiceDetail where CeveCode = '20702' and TransactionCode like '%STA%' order by TransactionCode --and TransactionCode in('STA_RES_19', 'STA_RES_20')
    //--insert into ServiceDetail
    //--values('STA_RES_19', 20702, '2023-10-05', 36894, 57, 'prueba', GETDATE(), 'prueba', GETDATE(), '2017-10-12 21:22:23')






    //        SELECT TransactionCode, ItemCode, CeveCode, SaleDate, Quantity
    //FROM ServiceDetail
    //where CeveCode = '20702' and SaleDate = '2023-10-05'
    //and TransactionCode in('STA_REC_14', 'STA_RES_19', 'STA_RES_20', 'PR_RES_100', 'PR_RES_104')
    //order by TRansactionCode,ItemCode;

    //--UPDATE ServiceDetail
    //--SET ItemCode = '123662'
    //--WHERE CeveCode = '20702' and ItemCode = '200477' and TransactionCode = 'PR_RES_100';


    // Assuming db is your DbContext instance

    private readonly DispatchBalanceContext _context;

    public TransferProductController(DispatchBalanceContext context)
    {
        _context = context;
    }

    // GET: api/SystemDates/5
    [ODataRoute("getTransferProduct")]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    [HttpGet("getTransferProduct")]
    public async Task<ActionResult<ProductsList>> GetTransferProduct(DateOnly saleDate, int ceveCode)
    {
       List<ProductsList.ProductListFinalGroup> results = new List<ProductsList.ProductListFinalGroup>();
        results = await (from p in _context.dbProductsList
                         join s in _context.dbServiceDetail on Convert.ToInt32(p.Internal_codes) equals s.ItemCode into sj
                         from s in sj.DefaultIfEmpty()
                         join i in _context.dbInventoryTransactionsCatalog on s.TransactionCode equals i.TransactionCode into ij
                         from i in ij.DefaultIfEmpty()
                         where (s.SaleDate == saleDate || s.SaleDate == null) &&
                               s.CeveCode == ceveCode &&
                               new[] { "STA_RES_19", "STA_RES_20" }.Contains(s.TransactionCode)
                         group new { p, s, i } by new
                         {
                             p.Internal_codes,
                             p.Large_name,
                             s.CeveCode,
                             s.SaleDate,
                             s.Quantity,
                             s.TransactionCode
                         } into grouped
                         select new ProductsList.ProductListFirstGroup
                         {
                             Internal_codes = grouped.Key.Internal_codes,
                             Large_name = grouped.Key.Large_name,
                             Entrada = grouped.Sum(x => x.s.Quantity > 0 ? x.s.Quantity : 0),
                             Salida = grouped.Sum(x => x.s.Quantity < 0 ? (x.s.Quantity *-1) : 0)
                         } into result
                         group result by new { result.Internal_codes, result.Large_name } into finalResult
                         select new ProductsList.ProductListFinalGroup
                         {
                             Internal_codes = finalResult.Key.Internal_codes,
                             Large_name = finalResult.Key.Large_name,
                             Entrada = finalResult.Sum(x => x.Entrada),
                             Salida = finalResult.Sum(x => x.Salida)
                         }).ToListAsync();

        var jsonMovements = JsonConvert.SerializeObject(results);
        if (results == null)
        {
            return NotFound();
        }

        return Ok(jsonMovements);
    }
}
