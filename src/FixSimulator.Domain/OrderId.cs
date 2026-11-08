namespace FixSimulator.Domain;

public readonly record struct OrderId(Guid Value)
{
    // Wrapping `Guid` in a record prevents wrong user input (e.g., swapping an order id object with a
    // trader id one (both would be Guid typed)

    /* The primary constructor used here is a C#12 feature. Prior version would implement it as
        public readonly record struct OrderId
        {
            public Guid Value {get; init; }

            public OrderId(Guid value)
            {
                Value = value;
            }
        }
    */

    // factory method - called as OrderId.New() since its static
    // also, `new` tells compiler that this is a OrderId instance (you don't have to repeat OrderId)
    public static OrderId New() => new(Guid.NewGuid());

    // overrides the ToString() built-in method for ease of readability for logging purposes
    public override string ToString() => Value.ToString();
}
