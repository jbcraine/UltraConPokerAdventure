using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIContestant : Contestant
{
    [SerializeField] private PokerAI _character;

    private void Awake()
    {
        _character = GetComponent<PokerAI>();
    }
    public override void MakeDecision(PokerState state)
    {
        return _character.MakeDecision();
    }


    protected virtual void EvaluateHand()
    {

    }

    protected void DeterminePossibleFutureHands()
    {

    }

}