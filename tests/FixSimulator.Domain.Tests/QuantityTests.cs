namespace FixSimulator.Domain.Tests;

public class QuantityTests
{
    // constructor

    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        var quantity = new Quantity(5);

        Assert.Equal(5, quantity.Value);
    }

    [Fact]
    public void Constructor_WithZeroValue_DoesNotThrow()
    {
        var exception = Record.Exception(() => new Quantity(0));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNegativeValue_ThrowsArgumentException(int value)
    {
        Assert.Throws<ArgumentException>(() => new Quantity(value));
    }

    // IsZero

    [Fact]
    public void IsZero_WhenValueIsZero_ReturnsTrue()
    {
        var quantity = new Quantity(0);

        Assert.True(quantity.IsZero);
    }

    [Fact]
    public void IsZero_WhenValuePositive_ReturnsFalse()
    {
        var quantity = new Quantity(5);

        Assert.False(quantity.IsZero);
    }

    // Zero constant

    [Fact]
    public void Zero_HasValueOfZero()
    {
        Assert.Equal(0, Quantity.Zero.Value);
    }

    [Fact]
    public void Zero_IsZero_ReturnsTrue()
    {
        Assert.True(Quantity.Zero.IsZero);
    }

    // Equality

    [Fact]
    public void Equality_TwoInstancesWithSameValue_AreEqual()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(5);

        Assert.Equal(q1, q2);
    }

    [Fact]
    public void Equality_TwoInstancesWithDifferentValues_AreNotEqual()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(3);

        Assert.NotEqual(q1, q2);
    }

    // Arithmetic operators

    [Fact]
    public void Addition_TwoQuantities_ReturnsSum()
    {
        var q1 = new Quantity(3);
        var q2 = new Quantity(2);

        var result = q1 + q2;

        Assert.Equal(new Quantity(5), result);
    }

    [Fact]
    public void Subtraction_LargerMinusSmaller_ReturnsCorrectResult()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(3);

        var result = q1 - q2;

        Assert.Equal(new Quantity(2), result);
    }

    [Fact]
    public void Subtraction_EqualQuantities_ReturnsZero()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(5);

        var result = q1 - q2;

        Assert.Equal(Quantity.Zero, result);
    }

    [Fact]
    public void Subtraction_ProducingNegativeResult_ThrowsArgumentException()
    {
        var q1 = new Quantity(2);
        var q2 = new Quantity(5);

        Assert.Throws<ArgumentException>(() => q1 - q2);
    }

    // ── comparison operators ───────────────────────────────────────────────

    [Fact]
    public void GreaterThan_WhenLeftIsHigher_ReturnsTrue()
    {
        var higher = new Quantity(5);
        var lower  = new Quantity(3);

        Assert.True(higher > lower);
    }

    [Fact]
    public void LessThan_WhenLeftIsLower_ReturnsTrue()
    {
        var lower  = new Quantity(3);
        var higher = new Quantity(5);

        Assert.True(lower < higher);
    }

    [Fact]
    public void GreaterThanOrEqual_WithEqualValues_ReturnsTrue()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(5);

        Assert.True(q1 >= q2);
    }

    [Fact]
    public void LessThanOrEqual_WithEqualValues_ReturnsTrue()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(5);

        Assert.True(q1 <= q2);
    }

    // ── IComparable ────────────────────────────────────────────────────────

    [Fact]
    public void CompareTo_SmallerQuantity_ReturnsNegative()
    {
        var smaller = new Quantity(3);
        var larger  = new Quantity(5);

        Assert.True(smaller.CompareTo(larger) < 0);
    }

    [Fact]
    public void CompareTo_EqualQuantity_ReturnsZero()
    {
        var q1 = new Quantity(5);
        var q2 = new Quantity(5);

        Assert.Equal(0, q1.CompareTo(q2));
    }

    [Fact]
    public void CompareTo_LargerQuantity_ReturnsPositive()
    {
        var larger  = new Quantity(5);
        var smaller = new Quantity(3);

        Assert.True(larger.CompareTo(smaller) > 0);
    }

    // ── ToString ───────────────────────────────────────────────────────────

    [Fact]
    public void ToString_ReturnsStringRepresentationOfValue()
    {
        var quantity = new Quantity(5);

        Assert.Equal("5", quantity.ToString());
    }
}
