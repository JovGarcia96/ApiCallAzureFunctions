using System.ComponentModel.DataAnnotations;

namespace DispatchBalanceAPI.Model
{
    public class DispatchBalanceFooter
    {
        [Key]
        public int CeveCode { get; set; }
        public DateOnly SaleDate { get; set; }
        public decimal InitialInventory { get; set; }
        public decimal RouteUnloading { get; set; }
        public decimal CeveTransfers { get; set; }
        public decimal ReturnTransfers { get; set; }
        public decimal[] ReceivedAsn { get; set; } = new decimal[0];
        public decimal ShortagesSurpluses { get; set; }
        public decimal AvailableInventory { get; set; }
        public decimal RouteDistribution { get; set; }
        public decimal IncomingIncidents { get; set; }
        public decimal OutgoingIncidents { get; set; }
        public decimal TotalInventory { get; set; }
    }
}
