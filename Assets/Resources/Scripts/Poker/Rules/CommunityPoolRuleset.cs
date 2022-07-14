using System.Collections.Generic;

public class CommunityPoolRuleset : PokerRuleset
{
    public int poolSize;
    public int phaseAmount;
    public List<int> cardsRevealedPerPhase;

    public CommunityPoolRuleset(int cardsPerContestant, long maxBet, int roundsToRaiseStartingBet, int poolSize, int phaseAmount, List<int> cardsRevealedPerPhase) : base(cardsPerContestant, maxBet, roundsToRaiseStartingBet)
    {
        this.poolSize = poolSize;
        this.phaseAmount = phaseAmount;
        this.cardsRevealedPerPhase = cardsRevealedPerPhase;
    }
}