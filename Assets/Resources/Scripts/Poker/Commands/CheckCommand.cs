using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCommandArgs
{
    public PokerType Game;

    public CheckCommandArgs(PokerType g)
    {
        Game = g;
    }
}
public class CheckCommand : PokerCommand
{
    private CheckCommandArgs args;

    public CheckCommand(CheckCommandArgs c)
    {
        args = c;
    }

    public override void Execute()
    {
        args.Game.Call(0);
    }
}
