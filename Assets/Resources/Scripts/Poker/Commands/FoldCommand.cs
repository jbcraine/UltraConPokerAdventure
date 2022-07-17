using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldCommandArgs
{
    public PokerType Game;

    public FoldCommandArgs(PokerType g)
    {
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
