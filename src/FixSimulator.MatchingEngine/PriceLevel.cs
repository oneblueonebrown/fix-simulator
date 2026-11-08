using FixSimulator.Domain;

namespace FixSimulator.MatchingEngine;

public class PriceLevel(Price price)
{
    // FIFO queue of a single price
    /*
     * Why LinkedList<Order> and not List<Order> or Queue<Order>
     * Queue<Order> is the obvious choice for FIFO — but it has no removal-by-ID operation.
     * When a cancel request arrives for an order sitting in the middle of a price level,
     * a Queue<T> cannot handle it without rebuilding the entire queue. List<Order> supports removal by index
     * but searching for an order by OrderId is O(n) and removal shifts all subsequent elements
    */

    private readonly LinkedList<Order> _orders = [];

    public Price Price { get; } = price;
    public bool IsEmpty => _orders.Count == 0;
    public int OrderCount => _orders.Count;
    public Quantity TotalQuantity =>
        new(_orders.Sum(o => o.RemainingQuantity.Value));

    public void AddOrder(Order order)
    {
        if (order.LimitPrice != Price)
            throw new ArgumentException(
                $"Order price {order.LimitPrice} does not match price level {Price}.",
                nameof(order));

        if (!order.IsActive)
            throw new ArgumentException(
                $"Cannot add order in terminal state '{order.Status}' to price level.",
                nameof(order));

        _orders.AddLast(order);
    }

    public Order? Peek() => _orders.First?.Value;

    public Order? Dequeue()
    {
        if (_orders.First is null)
            return null;

        var order = _orders.First.Value;
        _orders.RemoveFirst();
        return order;
    }

    public bool TryRemoveOrder(OrderId orderId)
    {
        var node = _orders.First;

        while (node is not null)
        {
            if (node.Value.Id == orderId)
            {
                _orders.Remove(node);
                return true;
            }

            node = node.Next;
        }

        return false;
    }

    public IReadOnlyList<Order> Orders =>
        _orders.ToList().AsReadOnly();
}
