using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFoldContestant : AIContestantController
{
    public override void MakeDecision(PokerState state)
    {
        DecisionMade( Fold(state) );
    }
}
