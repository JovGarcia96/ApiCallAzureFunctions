
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(CeveCode))]
public class MovementRoutingHeader
{
    public int CeveCode { get; set; }
    public string CeveName { get; set; } = "";
    public DateOnly SaleDate { get; set; }
    public string OrganizationCode { get; set; } = "";
}
