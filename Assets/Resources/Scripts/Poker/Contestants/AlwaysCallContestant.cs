using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlwaysCallContestant : AIContestant
{
    public override void MakeDecision(PokerState state)
    {
        DecisionMade( Call(state) );
    }
}
