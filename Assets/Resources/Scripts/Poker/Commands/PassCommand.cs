using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCommandArgs
{
    public Contestant Contestant;
    public PokerType Game;

    public PassCommandArgs(Contestant c, PokerType g)
    {
        Contestant = c;
        Game = g;
    }
}
public class PassCommand : PokerCommand
{
    public override void Execute()
    {
       
    }
}
