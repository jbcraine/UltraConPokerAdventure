using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseCommandArgs
{
    public Contestant Contestant;
    public int RaisedBet;
    public PokerType Game;

    public RaiseCommandArgs(Contestant c, int r, PokerType g)
    {
        Contestant = c;
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