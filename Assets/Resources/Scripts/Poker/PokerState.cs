using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerState
{
    public long CurrentBet;
    public long Pot;
    public PokerType Game;

    public PokerState(long bet, long pot, PokerType game)
    {
        CurrentBet = bet;
        Pot = pot;
        Game = game;
    }
}
