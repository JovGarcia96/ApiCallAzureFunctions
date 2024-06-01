namespace DispatchBalanceAPI.Model
{
    public class AcceptedASN
    {
        public string ReferenceNumber { get; set; } = "";
        public string OrganizationCode { get; set; } = "";   
        public DateOnly SaleDate { get; set; }
        public int CeveCode { get; set; }
        public DateTime AsnDate { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
        public int Shortages { get; set;} 
        public int Surpluses { get; set; }
    }
}
