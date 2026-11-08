namespace FixSimulator.Domain;

public readonly record struct Quantity : IComparable<Quantity>
{
    public static readonly Quantity Zero = new(0);

    public int Value { get; }

    public Quantity(int value)
    {
        if (value < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(value));
        Value = value;
    }

    public bool IsZero => Value == 0;
    public int CompareTo(Quantity other) => Value.CompareTo(other.Value);

    public static Quantity operator +(Quantity a, Quantity b) => new(a.Value + b.Value);
    public static Quantity operator -(Quantity a, Quantity b) => new(a.Value - b.Value);
    public static Quantity operator *(Quantity a, int scalar) => new(a.Value * scalar);
    public static bool operator >(Quantity a, Quantity b) => a.Value > b.Value;
    public static bool operator <(Quantity a, Quantity b) => a.Value < b.Value;
    public static bool operator >=(Quantity a, Quantity b) => a.Value >= b.Value;
    public static bool operator <=(Quantity a, Quantity b) => a.Value <= b.Value;

    public override string ToString() => Value.ToString();

}
