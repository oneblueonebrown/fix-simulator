namespace FixSimulator.Domain;

public enum ExecType
{
    New =         '0',
    PartialFill = '1',
    Fill =        '2',
    Cancelled =   '4',
    Rejected =    '8'
}
