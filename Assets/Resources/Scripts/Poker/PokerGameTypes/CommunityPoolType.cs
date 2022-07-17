using System.Collections.Generic;
public class CommunityPoolType : PokerType
{
    CommunityPool _Pool;
    protected int currentPhase = 0;

    protected CommunityPoolRuleset Rules { get { return (CommunityPoolRuleset)Ruleset; } }
    public PoolPokerContestant DowncastContestant(Contestant contestant)
    {
        PoolPokerContestant p = (PoolPokerContestant)contestant;
        return p;
    }

    public int CardsFlippedThisPhase
    {
        get { return Rules.cardsRevealedPerPhase[currentPhase-1]; }
    }

    public int TotalPhases { get { return Rules.cardsRevealedPerPhase.Count; } }
    public bool IsShowdownPhase { get { return currentPhase > TotalPhases; } }

    public CommunityPool Pool { get { return _Pool; } }

    public CommunityPoolType(CommunityPoolRuleset ruleset)
    {
        Ruleset = ruleset;
        _Pool = new CommunityPool();
    }

    public void Initialize(PokerGameInitializationInfo info, List<PoolPokerContestant> contestants)
    {
        List<Contestant> genericContestants = new List<Contestant>(contestants); 
        base.Initialize(info, genericContestants);
        _Contestants.ForEach(contestant => {
            DowncastContestant(contestant).InitializePool(_Pool);
        });
    }

    public override void StartMatch()
    {
        base.StartMatch();
    }

    public override void NextRound()
    {
        base.NextRound();
        currentPhase = 0;
        NextPhase();
    }

    public virtual void NextPhase()
    {
        currentPhase++;

        if (IsShowdownPhase)
        {
            AwardWinners(Showdown());
        }
        else
        {
            //Reveal pool cards
            Pool.RevealCards(CardsFlippedThisPhase);
        }
    }

}
