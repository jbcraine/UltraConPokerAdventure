using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCommandArgs
{
    Contestant Contestant;
    Card Card;
    int Price;
    PokerType Game;
}
public class BuyCommand : PokerCommand
{

    public BuyCommandArgs args;

    public BuyCommand(BuyCommandArgs b)
    {
        args = b;
    }
    public override void Execute()
    {

    }
}
