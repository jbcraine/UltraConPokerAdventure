using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ConPokerPhaseState
{
    public void StartPhase();
    public void StartTurn(Contestant contestant);

}
