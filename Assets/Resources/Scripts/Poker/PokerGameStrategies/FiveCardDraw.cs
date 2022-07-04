using System.Collections.Generic;

public class FiveCardDraw : PokerType
{
    public override long StartingBet { get; set; } = 500;
    protected override int _cardsPerContestant { get; } = 5;
    protected override int MaxPhases { get; } = 1;
    protected override int RoundsToRaiseStartingBet { get; } = 5;

    public FiveCardDraw (PokerGameInitializationInfo info, List<Contestant> contestants)
    {
        base.Initialize(info, contestants);
    }
}