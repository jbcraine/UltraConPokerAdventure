using System.Collections.Generic;

public class CommunityPoolRuleset : PokerRuleset
{
    public int poolSize;
    public List<int> cardsRevealedPerPhase;

    public CommunityPoolRuleset(int cardsPerContestant, long maxBet, int roundsToRaiseStartingBet, int poolSize, List<int> cardsRevealedPerPhase) : base(cardsPerContestant, maxBet, roundsToRaiseStartingBet)
    {
        this.poolSize = poolSize;
        this.cardsRevealedPerPhase = cardsRevealedPerPhase;
    }
}