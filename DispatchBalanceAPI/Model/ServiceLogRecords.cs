
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(LogId))]
public class ServiceLogRecords
{
    public int LogId { get; set; }
    public string LogEventCode { get; set; } = "";
    public DateOnly LogDate { get; set; }
    public int CeveCode { get; set; }
    public DateOnly SaleDate { get; set; }
    public string LogUser { get; set; } = "";
    public int LogStatusCode { get; set; }
    public string LogStatusResponse { get; set; } = "";
}

