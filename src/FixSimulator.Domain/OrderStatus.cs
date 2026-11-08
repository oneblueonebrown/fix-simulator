namespace FixSimulator.Domain;

public enum OrderStatus
{
    New = 0,
    PartiallyFilled = 1,
    Filled = 2,
    Cancelled = 4,
    Rejected = 8
}
