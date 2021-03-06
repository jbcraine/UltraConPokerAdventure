using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseCommandArgs
{
    public long RaisedBet;
    public PokerType Game;

    public RaiseCommandArgs(long r, PokerType g)
    {
        RaisedBet = r;
        Game = g;
    }
}
public class RaiseCommand : PokerCommand
{
    RaiseCommandArgs args;
    public RaiseCommand(RaiseCommandArgs raise)
    {
        args = raise;
    }

    public override void Execute()
    {
        args.Game.Raise(args.RaisedBet);
    }
}
