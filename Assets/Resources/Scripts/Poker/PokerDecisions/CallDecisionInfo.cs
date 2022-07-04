public class CallDecisionInfo : PokerDecisionInfo
{
    public long amount;
    public CallDecisionInfo(long amount)
    {
        moveType = PokerDecisionEnum.CALL;
        this.amount = amount;
    }
}