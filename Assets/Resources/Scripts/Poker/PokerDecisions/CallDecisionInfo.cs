public class CallDecisionInfo : PokerDecisionInfo
{
    public long amount;
    public CallDecisionInfo(long amount, Contestant contestant)
    {
        moveType = PokerDecisionEnum.CALL;
        this.amount = amount;
        this.contestant = contestant;
    }
}