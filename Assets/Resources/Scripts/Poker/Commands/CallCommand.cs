using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCommandArgs
{
    public Contestant Contestant;
    public int Contribution;
    public PokerType Game;

    public CallCommandArgs(Contestant c, int cont, PokerType game)
    {
        Contestant = c;
        Contribution = cont;
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
