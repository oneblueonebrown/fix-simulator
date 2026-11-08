using FixSimulator.Domain;

namespace FixSimulator.MatchingEngine;

public interface IMatchingStrategy
{
    // MatchingStrategy's sole responsibility is to generate Trades
    IReadOnlyList<Trade> Match(Order aggressiveOrder, OrderBook book);
}
