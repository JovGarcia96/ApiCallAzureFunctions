
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(TransactionCode))]
public class InventoryTransactionsCatalog
{
    public string TransactionCode { get; set; } = "";
    public string TransactionDescription { get; set; } = "";
    public char OutgoingIncome { get; set; }
    public string Type { get; set; } = "";
    public string ServiceCode { get; set; } = "";
    public DateTime CreatedOn {get;set;}
    public string CreatedBy { get; set; } = "";
    public DateTime ModifiedOn { get; set; }
    public string ModifiedBy { get; set; } = "";

}

