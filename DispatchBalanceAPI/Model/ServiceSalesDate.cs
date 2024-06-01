namespace DispatchBalanceAPI.Model
{
    public class ServiceSalesDate
    {
        public string ServiceCode { get; set; }
        public int CeveCode { get; set; }
        public DateOnly SaleDate { get; set; }
        public bool Required { get; set; }

    } 
}
