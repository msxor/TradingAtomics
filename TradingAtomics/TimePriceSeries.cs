﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exilion.TradingAtomics.Core;

namespace Exilion.TradingAtomics
{
    public class TimePriceSeries : TimeSeries<decimal>
    {
        public TimePriceSeries(Dictionary<DateTime, decimal> values) : base(values) { }
        public TimePriceSeries(IEnumerable<Tuple<DateTime, decimal>> values) : base(values) { }

        public PerformanceMetrics CalculatePerformanceMetrics()
        {
            if (Count == 0)
                return null;

            var pnlSeries = ToPnLSeries();

            var allValues = pnlSeries.Select(v => v.Value).ToList();
            var stdDev = allValues.StdDevP();
            var avgPnL = allValues.Average();
            var totalPnL = allValues.Sum();

            

            decimal sharpeRatio = 0;
            if (stdDev != 0)
                sharpeRatio = avgPnL / stdDev;
            var pm = new PerformanceMetrics
                (
                    sharpeRatio: sharpeRatio,
                    maxDrawdown: Decimal.Zero, 
                    maxDrawdownPc: Decimal.Zero, 
                    maxDrawdownRecoveryTime: TimeSpan.Zero, 
                    winningTrades: allValues.Count(v => v > 0),
                    loosingTrades: allValues.Count(v => v < 0),
                    totalPnL: totalPnL,
                    count: allValues.Count
                );
            pm = CalculateMaxDrawdownMetrics(pm);
            return pm;
        }

        private PerformanceMetrics CalculateMaxDrawdownMetrics(PerformanceMetrics pmIn)
        {
            DataPoint<decimal> peak = this.First();//.Clone(); 
            DataPoint<decimal> through = this.First();//.Clone();

            DataPoint<decimal> maxDrawdownFrom = peak;
            DataPoint<decimal> maxDrawdownTo = through;
            DataPoint<decimal> drawdownRecoveryPoint = peak;
            foreach (var current in this)
            {
                if (current.Value > peak.Value)
                {
                    // new peak found
                    // if through occured before peak, this is not drawdown
                    if (through.Time > peak.Time)
                    {
                        if (peak.Value - through.Value > maxDrawdownFrom.Value - maxDrawdownTo.Value)
                        {
                            // current drawdown exceeds previously recorded
                            maxDrawdownFrom = peak;
                            maxDrawdownTo = through;
                            drawdownRecoveryPoint = current;
                        }
                    }
                    peak = current;
                    through = current;  // reset through
                }

                if (current.Value < through.Value)
                    through = current;
            }
            // at the end, we must finalize last period
            if (through.Time > peak.Time)
            {
                if (peak.Value - through.Value > maxDrawdownFrom.Value - maxDrawdownTo.Value)
                {
                    // current drawdown exceeds previously recorded
                    maxDrawdownFrom = peak;
                    maxDrawdownTo = through;
                    drawdownRecoveryPoint = this.Last();
                }
            }

            var maxDrawdown = maxDrawdownFrom.Value - maxDrawdownTo.Value;
            var maxDrawdownPc = maxDrawdown / maxDrawdownFrom.Value;
            var maxDrawdownRecoveryTime = drawdownRecoveryPoint.Time - maxDrawdownTo.Time;


            PerformanceMetrics pm = new PerformanceMetrics
                (
                pmIn.SharpeRatio,
                maxDrawdown,
                maxDrawdownPc,
                maxDrawdownRecoveryTime,
                pmIn.WinningTrades,
                pmIn.LoosingTrades,
                pmIn.TotalPnL,
                pmIn.Count

                );
            return pm;
        }
        /// <summary>
        /// transforms from pnl series to cumulative series
        /// e.g. 1,2,-1,4 => 1,3,2,6
        /// </summary>
        /// <returns></returns>
        public static TimePriceSeries FromPnLSeries(TimePriceSeries pnlSeries)
        {
            decimal cumValue = 0;
            Dictionary<DateTime, decimal> newSeries = new Dictionary<DateTime, decimal>();

            foreach (var dataPoint in pnlSeries)
            {
                cumValue += dataPoint.Value;
                newSeries.Add(dataPoint.Time, cumValue);
            }

            TimePriceSeries ts = new TimePriceSeries(newSeries);
            return ts;
        }
        /// <summary>
        /// transforms to pnl series,
        /// e.g. 1,3,2,6 => 0,2,-1,4
        /// </summary>
        /// <returns></returns>
        public TimePriceSeries ToPnLSeries()
        {
            if (Count == 0)
                throw new ApplicationException("Data can't be null");

            Dictionary<DateTime, decimal> newSeries = new Dictionary<DateTime, decimal>();
            var prev = this.First().Value;
            foreach (var dataPoint in this)
            {
                newSeries.Add(dataPoint.Time, dataPoint.Value - prev);
                prev = dataPoint.Value;
            }

            TimePriceSeries ts = new TimePriceSeries(newSeries);
            return ts;
        }

    }
}
