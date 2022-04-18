using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldCommandArgs
{
    public Contestant Contestant;
    public PokerType Game;

    public FoldCommandArgs(Contestant c, PokerType g)
    {
        Contestant = c;
        Game = g;
    }
}
public class FoldCommand : PokerCommand
{
    private FoldCommandArgs args;

    public FoldCommand(FoldCommandArgs f)
    {
        args = f;
    }

    public override void Execute()
    {
        args.Game.Fold();
    }

}
