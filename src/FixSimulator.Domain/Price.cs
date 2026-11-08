namespace FixSimulator.Domain;

public readonly record struct Price: IComparable<Price>  // declaring that Price fulfils the contract defined by IComparable<Price>
{
    // Caveat about the constant below: its value is assigned at compile time
    // so if it is referenced in another project (e.g., Exchange) and we change the value below
    // we need to rebuild every project that references this constant
    public const decimal TickSize = 0.25m;

    public decimal Value { get; }  // no init setter - avoids constructor bypassing

    public Price(decimal value)
    {// can't use primary constructor here since it has no body (we need validation logic here)
        if (value <= 0)
            throw new ArgumentException("Price must be positive.", nameof(value));
        if (value % TickSize != 0)
            throw new ArgumentException("Price must be a multiple of tick size", nameof(value));

        Value = value;
    }

    // The `CompareTo` method below will be useful for ranking bids and ask prices in the order book
    public int CompareTo(Price other) => Value.CompareTo(other.Value);

    // without the overrides below, bidPrice > askPrice would not compile
    // similar to Python's __lt__ and __gt__
    public static bool operator >(Price a, Price b) => a.Value > b.Value;
    public static bool operator <(Price a, Price b) => a.Value < b.Value;
    public static bool operator >=(Price a, Price b) => a.Value >= b.Value;
    public static bool operator <=(Price a, Price b) => a.Value <= b.Value;

    public override string ToString() => Value.ToString("F2");

}
