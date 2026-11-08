using FixSimulator.Domain;

namespace FixSimulator.MatchingEngine;

public sealed class DescendingPriceComparer : IComparer<Price>
{
    public int Compare(Price x, Price y) => y.CompareTo(x);
}
