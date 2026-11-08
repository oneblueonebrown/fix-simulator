namespace FixSimulator.Domain.Tests;

public class OrderTests
{
    // helpers

    private static Order CreateLimitOrder(int quantity = 5, decimal price = 5000.25m)
        => new(
            clOrdId: "ORD001",
            symbol: "ES",
            side: Side.Buy,
            orderType: OrderType.Limit,
            timeInForce: TimeInForce.Day,
            quantity: new Quantity(quantity),
            limitPrice: new Price(price));

    private static Order CreateMarketOrder(int quantity = 5)
        => new(
            clOrdId: "ORD001",
            symbol: "ES",
            side: Side.Buy,
            orderType: OrderType.Market,
            timeInForce: TimeInForce.Day,
            quantity: new Quantity(quantity),
            limitPrice: null);

    // construction

    [Fact]
    public void Constructor_WithValidLimitOrder_SetsStatusToNew()
    {
        var order = CreateLimitOrder();

        Assert.Equal(OrderStatus.New, order.Status);
    }

    [Fact]
    public void Constructor_WithValidLimitOrder_SetsRemainingQuantityToOrderQuantity()
    {
        var order = CreateLimitOrder(quantity: 5);

        Assert.Equal(new Quantity(5), order.RemainingQuantity);
    }

    [Fact]
    public void Constructor_MarketOrderWithPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Order(
                clOrdId: "ORD001",
                symbol: "ES",
                side: Side.Buy,
                orderType: OrderType.Market,
                timeInForce: TimeInForce.Day,
                quantity: new Quantity(5),
                limitPrice: new Price(5000.25m)));
    }

    [Fact]
    public void Constructor_LimitOrderWithoutPrice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Order(
                clOrdId: "ORD001",
                symbol: "ES",
                side: Side.Buy,
                orderType: OrderType.Limit,
                timeInForce: TimeInForce.Day,
                quantity: new Quantity(5),
                limitPrice: null));
    }

    [Fact]
    public void Constructor_WithZeroQuantity_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Order(
                clOrdId: "ORD001",
                symbol: "ES",
                side: Side.Buy,
                orderType: OrderType.Limit,
                timeInForce: TimeInForce.Day,
                quantity: new Quantity(0),
                limitPrice: null));
    }

    // Fill

    [Fact]
    public void Fill_WithPartialQuantity_SetsStatusToPartiallyFilled()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(3));

        Assert.Equal(OrderStatus.PartiallyFilled, order.Status);
    }

    [Fact]
    public void Fill_WithFullQuantity_SetsStatusToFilled()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(5));

        Assert.Equal(OrderStatus.Filled, order.Status);
    }

    [Fact]
    public void Fill_WithPartialQuantity_UpdatesRemainingQuantityCorrectly()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(3));

        Assert.Equal(new Quantity(2), order.RemainingQuantity);
    }

    [Fact]
    public void Fill_MultipleTimes_AccumulatesFilledQuantity()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(3));
        order.Fill(new Quantity(2));

        Assert.Equal(new Quantity(5), order.FilledQuantity);
        Assert.Equal(OrderStatus.Filled, order.Status);
    }

    [Fact]
    public void Fill_WithQuantityExceedingRemaining_ThrowsException()
    {
        var order = CreateLimitOrder(quantity: 5);

        Assert.Throws<ArgumentException>(() => order.Fill(new Quantity(6)));
    }

    [Fact]
    public void Fill_WhenOrderIsCancelled_ThrowsInvalidOperationException()
    {
        var order = CreateLimitOrder();
        order.Cancel();

        Assert.Throws<InvalidOperationException>(() => order.Fill(new Quantity(1)));
    }

    [Fact]
    public void Fill_WhenOrderIsFilled_ThrowsInvalidOperationException()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(5));

        Assert.Throws<InvalidOperationException>(() => order.Fill(new Quantity(1)));
    }

    // cancel

    [Fact]
    public void Cancel_WhenOrderIsNew_SetsStatusToCancelled()
    {
        var order = CreateLimitOrder();
        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_WhenOrderIsPartiallyFilled_SetsStatusToCancelled()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(2));
        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_WhenOrderIsAlreadyCancelled_ThrowsInvalidOperationException()
    {
        var order = CreateLimitOrder();
        order.Cancel();

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    [Fact]
    public void Cancel_WhenOrderIsFilled_ThrowsInvalidOperationException()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(5));

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    // Reject

    [Fact]
    public void Reject_WhenOrderIsNew_SetsStatusToRejected()
    {
        var order = CreateLimitOrder();
        order.Reject("Invalid symbol.");

        Assert.Equal(OrderStatus.Rejected, order.Status);
    }

    [Fact]
    public void Reject_WhenOrderIsNew_SetsRejectionReason()
    {
        var order = CreateLimitOrder();
        order.Reject("Invalid symbol.");

        Assert.Equal("Invalid symbol.", order.RejectionReason);
    }

    [Fact]
    public void Reject_WhenOrderIsPartiallyFilled_ThrowsInvalidOperationException()
    {
        var order = CreateLimitOrder(quantity: 5);
        order.Fill(new Quantity(2));

        Assert.Throws<InvalidOperationException>(() => order.Reject("reason"));
    }

    // IsTerminal

    [Theory]
    [InlineData(5, 5, OrderStatus.Filled)]
    [InlineData(5, 3, OrderStatus.PartiallyFilled)]
    public void IsTerminal_AfterFill_ReturnsExpected(
        int orderQty, int fillQty, OrderStatus expectedStatus)
    {
        var order = CreateLimitOrder(quantity: orderQty);
        order.Fill(new Quantity(fillQty));
        Assert.Equal(expectedStatus, order.Status);
        Assert.Equal(expectedStatus == OrderStatus.Filled, order.IsTerminal);
    }

}
