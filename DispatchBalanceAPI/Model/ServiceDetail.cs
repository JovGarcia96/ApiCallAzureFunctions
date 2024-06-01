
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(TransactionCode), nameof(CeveCode), nameof(SaleDate), nameof(ItemCode))]
public class ServiceDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DetailId { get; set; }
    public string TransactionCode { get; set; }
    public int CeveCode { get; set; }
    public DateOnly SaleDate { get; set; }
    public int ItemCode { get; set; }
    public int Quantity { get; set; }
    public string AuditInfo_CreatedBy { get; set; }
    public string AuditInfo_CreatedOn { get; set; }
    public string AuditInfo_ModifiedBy { get; set; }
    public string AuditInfo_ModifiedOn { get; set; }
}
