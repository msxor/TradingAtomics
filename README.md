# TradingAtomics

Basic Timeseries and performance metrics objects to use in trading systems
Currently supported metrics:

+ _MaxDrawdown_
+ _Sharpe ratio_
* _WinningTrades_
* _LoosingTrades_
* _TotalPnL_


## Usage
```C#
TimePriceSeries priceSeries = new TimePriceSeries(new Dictionary<DateTime, decimal>
{
  {startTime.AddDays(1),5},
  {startTime.AddDays(2),6},
  {startTime.AddDays(3),7},
  {startTime.AddDays(4),6.5m},
  {startTime.AddDays(5),4},
});

var performanceMetrics = priceSeries.CalculatePerformanceMetrics();
```
