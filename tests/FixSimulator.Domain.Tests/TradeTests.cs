namespace FixSimulator.Domain.Tests;

public class TradeTests
{
    // helpers
    private static Trade CreateTrade(
        string? symbol = "ES",
        int quantity = 5,
        decimal price = 5000.25m)
        => new(
            buyOrderId: OrderId.New(),
            sellOrderId: OrderId.New(),
            symbol: symbol,
            price: new Price(price),
            quantity: new Quantity(quantity));

    // constructor

    [Fact]
    public void Constructor_WithValidArguments_DoesNotThrow()
    {
        var exception = Record.Exception(() => CreateTrade());

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithValidArguments_AssignsId()
    {
        var trade = CreateTrade();

        Assert.NotEqual(default, trade.Id);
        /* default for a readonly record struct is a zero-initialized
         * instance — in the case of TradeId, a TradeId wrapping Guid.Empty */
    }

    [Fact]
    public void Constructor_WithValidArguments_AssignsExecutedAt()
    {
        var before = DateTime.UtcNow;
        var trade = CreateTrade();
        var after = DateTime.UtcNow;

        Assert.InRange(trade.ExecutedAt, before, after);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithEmptyOrNullSymbol_ThrowsArgumentException(string? symbol)
    {
        Assert.Throws<ArgumentException>(() => CreateTrade(symbol: symbol));
    }

    [Fact]
    public void Constructor_WithZeroQuantity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => CreateTrade(quantity: 0));
    }

    // ── identity ───────────────────────────────────────────────────────────

    [Fact]
    public void TwoTrades_WithSameArguments_HaveDifferentIds()
    {
        var t1 = CreateTrade();
        var t2 = CreateTrade();

        Assert.NotEqual(t1.Id, t2.Id);
    }

    // ── immutability ───────────────────────────────────────────────────────

    [Fact]
    public void Constructor_SetsAllPropertiesCorrectly()
    {
        var buyOrderId  = OrderId.New();
        var sellOrderId = OrderId.New();
        var price       = new Price(5000.25m);
        var quantity    = new Quantity(5);

        var trade = new Trade(
            buyOrderId:  buyOrderId,
            sellOrderId: sellOrderId,
            symbol:      "ES",
            price:       price,
            quantity:    quantity);

        Assert.Equal(buyOrderId,  trade.BuyOrderId);
        Assert.Equal(sellOrderId, trade.SellOrderId);
        Assert.Equal("ES",        trade.Symbol);
        Assert.Equal(price,       trade.Price);
        Assert.Equal(quantity,    trade.Quantity);
    }
}
