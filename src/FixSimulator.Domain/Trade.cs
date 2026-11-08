namespace FixSimulator.Domain;

public record Trade
{
    public TradeId Id { get; }
    public OrderId BuyOrderId { get; }
    public OrderId SellOrderId { get; }
    public string Symbol { get; }
    public Price Price { get; }
    public Quantity Quantity { get; }
    public DateTime ExecutedAt { get; }

    public Trade(
        OrderId buyOrderId,
        OrderId sellOrderId,
        string? symbol,
        Price price,
        Quantity quantity)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol cannot be empty.", nameof(symbol));
        if (quantity.IsZero)
            throw new ArgumentException("Quantity cannot be zero.", nameof(quantity));

        Id          = TradeId.New();
        BuyOrderId  = buyOrderId;
        SellOrderId = sellOrderId;
        Symbol      = symbol;
        Price       = price;
        Quantity    = quantity;
        ExecutedAt  = DateTime.UtcNow;
    }
}
