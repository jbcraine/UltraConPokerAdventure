using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoCardDraw : PokerType
{
    public override long StartingBet { get; set; } = 1000;
    public override long CurrentBet { get; } = 0;
    public override long MainPot { get; } = 0;
    public override long MaxBet { get; set; } = 0;
}
