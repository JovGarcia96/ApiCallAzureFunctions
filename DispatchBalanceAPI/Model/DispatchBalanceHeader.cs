using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DispatchBalanceAPI.Model
{
    public class DispatchBalanceHeader
    {
        [Key]
        public int CeveCode { get; set; }
        public string CeveName { get; set; } = "";
        public DateOnly SaleDate { get; set; }
        public string OrganizationCode { get; set; } = "";
        public string[] ReceivedAsn { get; set; } = new string[0];
        public DateOnly Start { get; set; }
        public DateOnly Finish { get; set; }

    }
}



