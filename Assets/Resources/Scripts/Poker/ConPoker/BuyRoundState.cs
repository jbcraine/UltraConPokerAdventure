using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyRoundState : ConPokerPhaseState
{
    int decisionsMade = 0;
    private Hat _hat;

    public void StartPhase()
    {
        decisionsMade = 0;
    }

    public void StartTurn(Contestant contestant)
    {
        AskContestantForDecision(contestant);
    }

    public void BuyCards()
    {

    }

    public void DrawCards()
    {
        
    }

    public void Pass()
    {

    }

    public void SelectCards()
    {

    }

    public void AskContestantForDecision(Contestant contestant)
    {
       
    }
}
