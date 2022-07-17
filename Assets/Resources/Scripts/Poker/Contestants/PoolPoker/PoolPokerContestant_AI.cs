using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPoolPokerContestantController : AIContestantController
{
    PoolPokerContestantModel model;
    public void InitializePool(CommunityPool pool)
    {
        model.SetPool(pool);
    }

    protected void AddRevealedCommunityCardsToKnownCards(int n)
    {
        
    }
}
