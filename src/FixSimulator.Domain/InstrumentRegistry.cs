namespace FixSimulator.Domain;

public class InstrumentRegistry
{
    private readonly Dictionary<string, Instrument> _instruments = new();

    public void Register(Instrument instrument)
    {
        if (!_instruments.TryAdd(instrument.Symbol, instrument))
            throw new InvalidOperationException(
                $"Instrument '{instrument.Symbol}' is already registered.");
    }

    public Instrument GetBySymbol(string symbol)
    {
        if (_instruments.TryGetValue(symbol, out var instrument))
            return instrument;

        throw new KeyNotFoundException(
            $"Instrument '{symbol}' is not registered.");
    }

    public bool TryGetBySymbol(string symbol, out Instrument? instrument)
        => _instruments.TryGetValue(symbol, out instrument);

    public IReadOnlyCollection<Instrument> GetAll()
        => _instruments.Values.ToList().AsReadOnly();

    public static InstrumentRegistry CreateWithDefaults()
    {
        var registry = new InstrumentRegistry();

        registry.Register(new Instrument(
            symbol: "ES",
            description: "E-mini S&P 500 Futures",
            exchange: "CME",
            tickSize: 0.25m,
            tickValue: 12.50m,
            contractMultiplier: 50m,
            currency: "USD"));

        return registry;
    }
}
