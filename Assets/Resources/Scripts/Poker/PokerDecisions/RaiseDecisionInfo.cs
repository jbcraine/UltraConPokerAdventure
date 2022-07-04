public class RaiseDecisionInfo : PokerDecisionInfo
{
    public long bet;
    public RaiseDecisionInfo (long bet, Contestant contestant)
    {
        moveType = PokerDecisionEnum.RAISE;
        this.bet = bet;
        this.contestant = contestant;
    }
}
