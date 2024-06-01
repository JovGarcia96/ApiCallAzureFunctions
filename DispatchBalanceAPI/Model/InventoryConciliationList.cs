
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace DispatchBalanceAPI.Model;

public class InventoryConciliationList
{
    public class InventoryConciliationListFinalGroup
    {
        public string Internal_codes { get; set; } = "";
        public string Large_name { get; set; } = "";
        public string TransactionCode { get; set; } = "";
        public int Inventario_Servicios { get; set; }
        public int Inventario_Corte { get; set; }
    }
}
