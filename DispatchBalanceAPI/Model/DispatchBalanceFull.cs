namespace DispatchBalanceAPI.Model
{
    public class DispatchBalanceFull
    {
        public DispatchBalanceFull()
        {
        }
        public DispatchBalanceHeader header { get; set; }
        public List<DispatchBalanceItem> items { get; set; }

        public DispatchBalanceFooter total { get; set; }
    }
}
