using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolPokerContestantModel : ContestantModel
{
    protected CommunityPool _pool;
    
    public CommunityPool Pool { get { return _pool; } }
    public void SetPool (CommunityPool pool)
    {
        _pool = pool;
    }
}
