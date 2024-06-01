
﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace DispatchBalanceAPI.Model;

public class MirrorList
{
    public class MirrorListFinalGroup
    {
        public int ItemCode { get; set; }
        public string Nombre { get; set; } = "";
        public int Entrada { get; set; }
        public int Salida { get; set; }
    }
}
