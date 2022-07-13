using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPokerContestant : Contestant
{
    new PoolPokerContestantModel _model;
    new AIPoolPokerContestantController _controller;

    public virtual void InitializePool(CommunityPool pool)
    {
        _model.SetPool(pool);
    }
}
