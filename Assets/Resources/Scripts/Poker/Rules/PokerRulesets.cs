using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PokerRulesets
{
    public static readonly CommunityPoolRuleset Texas_Holdem = new CommunityPoolRuleset(cardsPerContestant: 2, maxBet: long.MaxValue, roundsToRaiseStartingBet: 5, poolSize: 5, phaseAmount: 4, cardsRevealedPerPhase: new List<int>() { 0, 3, 1, 1 });
}
