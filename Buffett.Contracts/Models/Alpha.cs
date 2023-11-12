namespace Buffett.Contracts.Models
{
    public class Alpha
    {
        public decimal ActualReturn { get; set; }
        public decimal MarketReturn { get; set; }
        public decimal RiskFreeReturn { get; set; }
        public decimal Beta { get; set; }
        public decimal AlphaValue { get; set; }
    }
}
