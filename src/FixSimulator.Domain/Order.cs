using System.Security.Cryptography.X509Certificates;

namespace FixSimulator.Domain;

public class Order
{
    public OrderId Id { get; }
    public string ClOrdId { get; }
    public string Symbol { get; }
    public Side Side { get; }

    public OrderType OrderType { get; }
    public TimeInForce TimeInForce { get; }
    public Price? LimitPrice { get; }
    public Quantity Quantity { get; }
    public Quantity FilledQuantity { get; private set; }
    public Quantity RemainingQuantity => new(Quantity.Value - FilledQuantity.Value);
    public OrderStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }
    public DateTime CreatedAt { get; }

    public bool IsTerminal => Status is OrderStatus.Filled
        or OrderStatus.Cancelled
        or OrderStatus.Rejected;
    public bool IsActive => !IsTerminal;

    public Order(
        string clOrdId,
        string symbol,
        Side side,
        OrderType orderType,
        TimeInForce timeInForce,
        Quantity quantity,
        Price? limitPrice = null)
    {
        if (string.IsNullOrWhiteSpace(clOrdId))
            throw new ArgumentException("ClOrdId cannot be empty.", nameof(clOrdId));
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol cannot be empty.", nameof(symbol));
        if (quantity.IsZero)
            throw new ArgumentException("Order quantity cannot be zero.", nameof(quantity));
        if (orderType == OrderType.Limit && limitPrice is null)
            throw new ArgumentException("Limit orders must have a non-null price.", nameof(limitPrice));
        if (orderType == OrderType.Market && limitPrice is not null)
            throw new ArgumentException("Market orders cannot have a price limit.", nameof(limitPrice));

        Id = OrderId.New();
        ClOrdId = clOrdId;
        Symbol = symbol;
        Side = side;
        OrderType = orderType;
        TimeInForce = timeInForce;
        Quantity = quantity;
        LimitPrice = limitPrice;
        FilledQuantity = Quantity.Zero;
        Status = OrderStatus.New;
        CreatedAt = DateTime.UtcNow;
    }

    public void Fill(Quantity fillQuantity)
    {
        GuardAgainstTerminalState(nameof(Fill));

        if (fillQuantity.IsZero)
            throw new ArgumentException("Fill quantity cannot be zero.", nameof(fillQuantity));

        if (fillQuantity > RemainingQuantity)
            throw new ArgumentException("Fill quantity exceeds remaining quantity", nameof(fillQuantity));

        FilledQuantity = FilledQuantity + fillQuantity;
        Status = RemainingQuantity.IsZero
            ? OrderStatus.Filled
            : OrderStatus.PartiallyFilled;
    }

    public void Cancel()
    {
        GuardAgainstTerminalState(nameof(Cancel));
        Status = OrderStatus.Cancelled;
    }

    public void Reject(string reason)
    {
        if (Status != OrderStatus.New)
            throw new InvalidOperationException(
                $"Cannot reject an order in status '{Status}'");

        RejectionReason = reason;
        Status          = OrderStatus.Rejected;
    }

    private void GuardAgainstTerminalState(string operation)
    {
        if (IsTerminal)
            throw new InvalidOperationException(
                $"Cannot apply '{operation}' to an order in terminal state '{Status}.");
    }
}
