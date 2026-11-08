namespace FixSimulator.Domain;

public readonly record struct ExecutionReportId(Guid Value)
{
    public static ExecutionReportId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
