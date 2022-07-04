using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPokerContestant : Contestant
{
    protected CommunityPool _Pool;

    public virtual void InitializePool(CommunityPool pool)
    {
        _Pool = pool;
    }
}
