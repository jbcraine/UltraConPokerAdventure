using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPoolPokerContestantController : AIContestantController
{
    CommunityPool _pool;
    public void InitializePool(CommunityPool pool)
    {
       _pool = pool;
    }

    protected void AddRevealedCommunityCardsToKnownCards(int n)
    {
        
    }
}
