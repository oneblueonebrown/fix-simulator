namespace FixSimulator.Domain;

public readonly record struct TradeId(Guid Value)
{
    public static TradeId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
