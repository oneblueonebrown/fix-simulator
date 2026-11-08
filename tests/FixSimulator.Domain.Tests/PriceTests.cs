namespace FixSimulator.Domain.Tests;

public class PriceTests
{
    // Constructor

    [Fact]
    public void Constructor_WithValidPrice_SetsValue()
    {
        var price = new Price(5000.25m);

        Assert.Equal(5000.25m, price.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Constructor_WithNonPositiveValue_ThrowsArgumentException(decimal value)
    {
        Assert.Throws<ArgumentException>(() => new Price(value));
    }

    [Theory]
    [InlineData(5000.10)]
    [InlineData(5000.13)]
    [InlineData(4999.99)]
    public void Constructor_WithInvalidTickSize_ThrowsArgumentException(decimal value)
    {
        Assert.Throws<ArgumentException>(() => new Price(value));
    }

    [Theory]
    [InlineData(5000.00)]
    [InlineData(5000.25)]
    [InlineData(5000.50)]
    [InlineData(5000.75)]
    [InlineData(4999.75)]
    public void Constructor_WithValidTickAlignedPrice_DoesNotThrow(decimal value)
    {
        var exception = Record.Exception(() => new Price(value));

        Assert.Null(exception);
    }

    // Equality

    [Fact]
    public void Equality_TwoInstancesWithSameValue_AreEqual()
    {
        var p1 = new Price(5000.25m);
        var p2 = new Price(5000.25m);

        Assert.Equal(p1, p2);
    }

    [Fact]
    public void Equality_TwoInstancesWithDifferentValues_AreNotEqual()
    {
        var p1 = new Price(5000.25m);
        var p2 = new Price(5000.50m);

        Assert.NotEqual(p1, p2);
    }

    // comparison operators

    [Fact]
    public void GreaterThan_WhenLeftIsHigher_ReturnsTrue()
    {
        var higher = new Price(5000.50m);
        var lower  = new Price(5000.25m);

        Assert.True(higher > lower);
    }

    [Fact]
    public void LessThan_WhenLeftIsLower_ReturnsTrue()
    {
        var lower  = new Price(5000.25m);
        var higher = new Price(5000.50m);

        Assert.True(lower < higher);
    }

    [Fact]
    public void GreaterThanOrEqual_WithEqualValues_ReturnsTrue()
    {
        var p1 = new Price(5000.25m);
        var p2 = new Price(5000.25m);

        Assert.True(p1 >= p2);
    }

    [Fact]
    public void LessThanOrEqual_WithEqualValues_ReturnsTrue()
    {
        var p1 = new Price(5000.25m);
        var p2 = new Price(5000.25m);

        Assert.True(p1 <= p2);
    }

    // IComparable

    [Fact]
    public void CompareTo_LowerPrice_ReturnsNegative()
    {
        var lower = new Price(5000.25m);
        var higher = new Price(5000.50m);

        Assert.True(lower.CompareTo(higher) < 0);
    }

    [Fact]
    public void CompareTo_EqualPrice_ReturnsZero()
    {
        var p1 = new Price(5000.25m);
        var p2 = new Price(5000.25m);

        Assert.Equal(0, p1.CompareTo(p2));
    }

    [Fact]
    public void CompareTo_HigherPrice_ReturnsPositive()
    {
        var lower = new Price(5000.25m);
        var higher = new Price(5000.50m);

        Assert.True(higher.CompareTo(lower) > 0);
    }

    // Sorting

    [Fact]
    public void Sort_ListOfPrices_SortsAscending()
    {
        var prices = new List<Price>
        {
            new Price(5001.00m),
            new Price(4999.75m),
            new Price(5000.25m),
            new Price(5000.00m)
        };

        /*.Order() is a LINQ extension method introduced in .NET 7 — shorthand for .OrderBy(x => x).
         It works because Price implements IComparable<Price>, which gives LINQ the information it needs to
         rank instances without an explicit key selector.*/

        var sorted = prices.Order().ToList();

        Assert.Equal(new Price(4999.75m), sorted[0]);
        Assert.Equal(new Price(5000.00m), sorted[1]);
        Assert.Equal(new Price(5000.25m), sorted[2]);
        Assert.Equal(new Price(5001.00m), sorted[3]);
    }

    // ToString

    [Fact]
    public void ToString_ReturnsValueFormattedToTwoDecimalPlaces()
    {
        var price = new Price(5000.25m);

        Assert.Equal("5000.25", price.ToString());
    }
}
