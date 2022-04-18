using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConPokerContestant : Contestant
{
    //End the turn without doing anything
    protected virtual void Pass()
    {

    }

    //Select cards to remove and specifically select replacements
    protected virtual void Buy()
    {

    }

    //Select cards to remove and randomly draw replacements
    protected virtual void Draw()
    {

    }
}
