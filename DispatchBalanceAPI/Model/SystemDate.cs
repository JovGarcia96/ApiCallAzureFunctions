using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DispatchBalanceAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace DispatchBalanceAPI.Model
{
    public class SystemDate
    {
        public int CeveCode { get; set; }
        public DateOnly SaleDate  { get; set; }
        public char Status { get; set; }

    }
}
