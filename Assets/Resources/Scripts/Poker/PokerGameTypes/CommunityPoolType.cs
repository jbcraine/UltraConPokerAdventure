using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityPoolType : PokerType
{
    new CommunityPoolRuleset Ruleset;
    CommunityPool _Pool;
    new List<PoolPokerContestant> _Contestants;

    public CommunityPool Pool { get { return _Pool; } }


    public CommunityPoolType(CommunityPoolRuleset ruleset)
    {
        Ruleset = ruleset;
    }
    public override void Initialize(PokerGameInitializationInfo info, List<Contestant> contestants)
    {
        base.Initialize(info, contestants);
        _Pool = new CommunityPool();
        _Contestants.ForEach(contestant => contestant.InitializePool(_Pool));
    }

    public override void StartMatch()
    {
        base.StartMatch();
    }
}
