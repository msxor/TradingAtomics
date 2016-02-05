namespace Exilion.TradingAtomics
{
    public class PerformanceMetrics
    {
        public PerformanceMetrics(decimal sharpeRatio, decimal maxDrawdown, decimal winningTrades, decimal loosingTrades, decimal totalPnL, long count)
        {
            SharpeRatio = sharpeRatio;
            MaxDrawdown = maxDrawdown;
            WinningTrades = winningTrades;
            LoosingTrades = loosingTrades;
            TotalPnL = totalPnL;
            Count = count;
        }
        public long Count { get; private set; }
        public decimal SharpeRatio { get; private set; } 
        public decimal MaxDrawdown{ get; private set; } 
        public decimal WinningTrades { get; private set; }
        public decimal LoosingTrades { get; private set; }
        public decimal TotalPnL { get; private set; } 
    }
}
