public class FoldDecisionInfo : PokerDecisionInfo
{
    public FoldDecisionInfo(Contestant contestant)
    {
        moveType = PokerDecisionEnum.FOLD;
        this.contestant = contestant;
    }
}