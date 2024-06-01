using System.ComponentModel.DataAnnotations;

namespace DispatchBalanceAPI.Model
{
    public class DispatchBalanceItem
    {
        [Key]
        public int LineNumber { get; set; }
        public int CeveCode { get; set; }
        public DateOnly SaleDate { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; } = "";
        public int TrayCapacity { get; set; }
        public decimal UnitPrice { get; set; }
        public int InitialInventory { get; set; }
        public int RouteUnloading { get; set; }
        public int CeveTransfers { get; set; }
        public int ReturnTransfers { get; set; }
        public int[] ReceivedAsn { get; set; } = new int[0];
        public int ShortagesSurpluses { get; set; }
        public int AvailableInventory { get; set; }
        public int RouteDistribution { get; set; }
        public int IncomingIncidents { get; set; }
        public int OutgoingIncidents { get; set; }
        public int TotalInventory { get; set; }

    }
}
