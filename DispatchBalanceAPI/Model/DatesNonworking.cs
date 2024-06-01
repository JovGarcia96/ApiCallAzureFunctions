
﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace DispatchBalanceAPI.Model;

[PrimaryKey(nameof(CountryCode), nameof(OrganizationCode), nameof(CeveCode), nameof(DateNonworking))]
public class DatesNonworking
{
    public string CountryCode { get; set; }
    public string OrganizationCode { get; set; }
    public string CeveCode { get; set; }
    public string DateNonworking { get; set; }
}
