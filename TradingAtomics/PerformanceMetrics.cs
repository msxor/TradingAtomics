namespace TradingAtomics
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
        public decimal SharpeRatio { get; private set; } // pnl
        public decimal MaxDrawdown{ get; private set; } // price
        public decimal WinningTrades { get; private set; } // pnl
        public decimal LoosingTrades { get; private set; } // pnl
        public decimal TotalPnL { get; private set; } // pnl
    }
}
