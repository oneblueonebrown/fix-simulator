namespace FixSimulator.Domain;

public record Instrument
{
    public string Symbol { get; }
    public string Description { get; }
    public string Exchange { get; }
    public decimal TickSize { get; }
    public decimal TickValue { get; }
    public decimal ContractMultiplier { get; }
    public string Currency { get; }

    public Instrument(
        string symbol,
        string description,
        string exchange,
        decimal tickSize,
        decimal tickValue,
        decimal contractMultiplier,
        string currency)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol cannot be empty.", nameof(symbol));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        if (string.IsNullOrWhiteSpace(exchange))
            throw new ArgumentException("Exchange cannot be empty.", nameof(exchange));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.", nameof(currency));

        if (tickSize <= 0)
            throw new ArgumentException("Tick size must be positive.", nameof(tickSize));
        if (tickValue <= 0)
            throw new ArgumentException("Tick value must be positive.", nameof(tickValue));
        if (contractMultiplier <= 0)
            throw new ArgumentException("Contract multiplier must be positive.", nameof(contractMultiplier));

        Symbol             = symbol;
        Description        = description;
        Exchange           = exchange;
        TickSize           = tickSize;
        TickValue          = tickValue;
        ContractMultiplier = contractMultiplier;
        Currency           = currency;
    }

    public bool IsPriceValid(Price price) => price.Value % TickSize == 0;
    public override string ToString() => $"{Symbol} ({Exchange})";
}
