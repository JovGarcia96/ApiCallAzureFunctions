
﻿using Microsoft.EntityFrameworkCore;

namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(TransactionCode), nameof(CeveCode), nameof(TransactionDescription))]
public class MovementRoutingItemsOutput
{
    public string TransactionCode { get; set; }
    public string TransactionDescription { get; set; }
    public int CeveCode { get; set; }
    public DateOnly SaleDate { get; set; }
    public int Quantity { get; set; }
    public char OutgoingIncome { get; set; }
}
