using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCommandArgs
{
    public PokerType Game;

    public PassCommandArgs(PokerType g)
    {
        Game = g;
    }
}
public class PassCommand : PokerCommand
{
    public override void Execute()
    {
       
    }
}
