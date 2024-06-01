
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace DispatchBalanceAPI.Model;

public class TransferProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DetailId { get; set; }
    public string TransactionCode { get; set; }
    public int CeveCode { get; set; }
    public DateOnly SaleDate { get; set; }
    public int ItemCode { get; set; }
    public int Quantity { get; set; }
    
}

