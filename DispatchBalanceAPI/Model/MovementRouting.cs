
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(TransactionCode), nameof(CeveCode), nameof(TransactionDescription))]
public class MovementRouting
{
    //public MovementRouting()
    //{
    //}

    //public MovementRoutingHeader header { get; set; }
    //public List<MovementRoutingItemsInput> itemsInput { get; set; }
    //public List<MovementRoutingItemsOutput> itemsOutput { get; set; }
    //public double totalInputs { get; set; }
    //public double totalOutputs { get; set; }

    public int Quantity { get; set; }
    public string TransactionCode { get; set; }
    public int CeveCode { get; set; }
    public DateOnly SaleDate { get; set; }
    public string TransactionDescription { get; set; }
    public char OutgoingIncome { get; set; }
}
