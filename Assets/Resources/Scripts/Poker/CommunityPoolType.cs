using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityPoolType : PokerType
{
    public override float ScoreHand(Hand hand)
    {
        return ScoreHand(hand, null);
    }

    private float ScoreHand(Hand hand, CommunityPool pool)
    {
        return 0f;
    }
}
