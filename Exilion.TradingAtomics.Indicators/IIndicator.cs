using System.Reactive.Subjects;

namespace Exilion.TradingAtomics.Indicators
{
    public interface IIndicator : ISubject<DataPoint<decimal>>
    {

    }
}
