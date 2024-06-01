
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DispatchBalanceAPI.Model;

//AG-GRID COMENTAR ESTAS LÍNEAS 7-21
//public class Records
//{
//    [Key]
//    public string TransactionCode { get; set; } = "";
//    public string TransactionDescription { get; set; } = "";
//    public string OutgoingIncome { get; set; } = "";
//    public string Type { get; set; } = "";
//    public string ServiceCode { get; set; } = "";
//}

//public class InventoryTransactionsRecords
//{

//    public List<Records> records { get; set; }
//}



//AG-GRID DESCOMENTAR ESTAS LÍNEAS
[PrimaryKey(nameof(TransactionCode))]
public class InventoryTransactionsRecords
{
    public string TransactionCode { get; set; } = "";
    public string TransactionDescription { get; set; } = "";
    public string OutgoingIncome { get; set; } = "";
    public string Type { get; set; } = "";
    public string ServiceCode { get; set; } = "";
}
