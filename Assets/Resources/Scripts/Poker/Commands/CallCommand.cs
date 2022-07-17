using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCommandArgs
{
    public long Contribution;
    public PokerType Game;

    public CallCommandArgs(long contribution, PokerType game)
    {
        Contribution = contribution;
        Game = game;
    }
}
public class CallCommand : PokerCommand
{
    private CallCommandArgs args;

    public CallCommand(CallCommandArgs c)
    {
        args = c;
    }
    public override void Execute()
    {
        args.Game.Call(args.Contribution);
    }
}
