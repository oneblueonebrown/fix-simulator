namespace FixSimulator.Domain;

public record ExecutionReport
{
    public ExecutionReportId Id { get; }
    public OrderId OrderId { get; }
    public string ClOrdId { get; }
    public string Symbol { get; }
    public Side Side { get; }
    public OrderStatus OrderStatus { get; }
    public ExecType ExecType { get; }
    public Quantity OrderQuantity { get; }
    public Quantity FilledQuantity { get; }
    public Quantity RemainingQuantity { get; }
    public Price? LastPrice { get; }
    public Quantity? LastQuantity { get; }
    public string? RejectionReason { get; }
    public DateTime TransactTime { get; }

    public ExecutionReport(
        OrderId orderId,
        string clOrdId,
        string symbol,
        Side side,
        OrderStatus orderStatus,
        ExecType execType,
        Quantity orderQuantity,
        Quantity filledQuantity,
        Quantity remainingQuantity,
        Price? lastPrice = null,
        Quantity? lastQuantity = null,
        string? rejectionReason = null)
    {
        if (string.IsNullOrWhiteSpace(clOrdId))
            throw new ArgumentException("ClOrdId cannot be empty.", nameof(clOrdId));
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol cannot be empty.", nameof(symbol));

        Id                = ExecutionReportId.New();
        OrderId           = orderId;
        ClOrdId           = clOrdId;
        Symbol            = symbol;
        Side              = side;
        OrderStatus       = orderStatus;
        ExecType          = execType;
        OrderQuantity     = orderQuantity;
        FilledQuantity    = filledQuantity;
        RemainingQuantity = remainingQuantity;
        LastPrice         = lastPrice;
        LastQuantity      = lastQuantity;
        RejectionReason   = rejectionReason;
        TransactTime      = DateTime.UtcNow;
    }
}
