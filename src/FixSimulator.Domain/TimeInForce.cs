namespace FixSimulator.Domain;

public enum TimeInForce
{
    Day = 0,  // orders good until trading day is over
    GoodTillCancel = 1,  // good until order is canceled by trader
    AtTheOpening = 2,  // to be executed when markets open
    ImmediateOrCancel = 3,  // fill whatever is possible and cancel the balance
    FillOrKill = 4  // fill the entire quantity or cancel
}
