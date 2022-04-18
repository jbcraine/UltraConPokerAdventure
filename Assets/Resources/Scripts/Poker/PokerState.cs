using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerState
{
    public int CurrentBet;
    public int Pot;
    public PokerType Game;

    public PokerState(int bet, int pot, PokerType game)
    {
        CurrentBet = bet;
        Pot = pot;
        Game = game;
    }
}
