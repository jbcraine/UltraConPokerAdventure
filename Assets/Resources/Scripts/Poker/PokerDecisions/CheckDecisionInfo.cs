public class CheckDecisionInfo : PokerDecisionInfo
{
    public CheckDecisionInfo(Contestant contestant)
    {
        moveType = PokerDecisionEnum.CHECK;
        this.contestant = contestant;
    }
}