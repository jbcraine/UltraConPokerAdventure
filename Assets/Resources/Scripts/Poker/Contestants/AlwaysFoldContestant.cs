using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFoldContestant : AIContestant
{
    public override void MakeDecision(PokerState state)
    {
        DecisionMade( Fold(state) );
    }
}
