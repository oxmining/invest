namespace OX.Web
{
    public class OTCSaleViewModel
    {
        public string PoolEthAddress { get; set; }
        public UInt160 InPoolAddress { get; set; }
        public decimal Amount { get; set; }
        public int FeeRatio { get; set; }
        public int State { get; set; }
    }
}
