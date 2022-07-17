using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCommandArgs
{ 
    public int ReplacementCount;
    public PokerType Game;
}
public class DrawCommand : PokerCommand
{
    public DrawCommandArgs args;

    public DrawCommand(DrawCommandArgs d)
    {
        args = d;
    }

    public override void Execute()
    {
       
    }

}
