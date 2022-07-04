public class RaiseDecisionInfo : PokerDecisionInfo
{
    public long bet;
    public RaiseDecisionInfo (long bet)
    {
        moveType = PokerDecisionEnum.RAISE;
        this.bet = bet;
    }
}
