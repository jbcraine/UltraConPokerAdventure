
public class PokerRuleset
{
    public int cardsPerContestant;
    public long maxBet;
    public int roundsToRaiseStartingBet;

    public PokerRuleset(int cardsPerContestant, long maxBet, int roundsToRaiseStartingBet)
    {
        this.cardsPerContestant = cardsPerContestant;
        this.maxBet = maxBet;
        this.roundsToRaiseStartingBet = roundsToRaiseStartingBet;
    }
}
