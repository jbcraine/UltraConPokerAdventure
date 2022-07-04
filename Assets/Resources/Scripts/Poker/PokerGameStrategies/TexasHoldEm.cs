using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexasHoldEm : PokerType
{
    protected private int _CurrentPhase = 0;
    protected override int MaxPhases { get; } = 4;
    protected override int RoundsToRaiseStartingBet { get; } = 5;
    protected override int _cardsPerContestant { get; } = 2;
    protected int _CommunityPoolSize = 5;
    protected CommunityPool _CommunityPool;

    public TexasHoldEm(PokerGameInitializationInfo info, List<Contestant> contestants)
    {
        Initialize(info, contestants);
    }
    public override void Initialize(PokerGameInitializationInfo info, List<Contestant> contestants)
    {
        base.Initialize(info, contestants);
        _CommunityPool = new CommunityPool();
    }

    public override void StartMatch()
    {
        base.StartMatch();
    }

    public override void NextRound()
    {
        _CurrentPhase = 1;
        base.NextRound();
    }

    public override void NextTurn(Contestant contestant)
    {
        base.NextTurn(contestant);
    }

    public override void NextPhase()
    {
        _CurrentPhase++;
        base.NextPhase();
    }

    protected override void PhaseCheck()
    {
        switch (_CurrentPhase)
        {
            case 2: Flop(); break;
            case 3: Turn(); break;
            case 4: River(); break;
            case 5: AwardWinners(Showdown()); EndRound(); break;
            default: break;
        }
    }

    private void Flop()
    {
        _CommunityPool.RevealCards(3);
    }

    private void Turn()
    {
        _CommunityPool.RevealCards(1);
    }

    private void River()
    {
        _CommunityPool.RevealCards(1);
    }
}
