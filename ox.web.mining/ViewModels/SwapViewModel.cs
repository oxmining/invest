namespace OX.Web
{
    public class SwapViewModel
    {
        public uint Amount { get; set; }
    }
    public class StockItem
    {
        public string ts_code { get; set; }
        public string trade_date { get; set; }
        public double close { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double vol { get; set; }
        public double amount { get; set; }
    }
     
}
