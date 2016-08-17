using System;

namespace Exilion.TradingAtomics
{
    public class PerformanceMetrics
    {
        public PerformanceMetrics(
            decimal sharpeRatio, 
            decimal maxDrawdown,
            decimal maxDrawdownPc,
            TimeSpan maxDrawdownRecoveryTime,
            decimal winningTrades, 
            decimal loosingTrades, 
            decimal totalPnL, 
            long count)
        {
            SharpeRatio = sharpeRatio;
            MaxDrawdown = maxDrawdown;
            MaxDrawdownPc = maxDrawdownPc;
            MaxDrawdownRecoveryTime = maxDrawdownRecoveryTime;
            WinningTrades = winningTrades;
            LoosingTrades = loosingTrades;
            TotalPnL = totalPnL;
            Count = count;
        }
        public long Count { get; private set; }
        public decimal SharpeRatio { get; private set; } 
        public decimal MaxDrawdown{ get; private set; }
        public decimal MaxDrawdownPc { get; private set; }
        public TimeSpan MaxDrawdownRecoveryTime { get; private set; }
        public decimal WinningTrades { get; private set; }
        public decimal LoosingTrades { get; private set; }
        public decimal TotalPnL { get; private set; } 
    }
}
