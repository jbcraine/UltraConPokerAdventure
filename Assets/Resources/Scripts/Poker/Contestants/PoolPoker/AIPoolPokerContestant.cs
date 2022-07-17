using UnityEngine;

public class AIPoolPokerContestant : PoolPokerContestant
{
    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<AIPoolPokerController>();
        Debug.Log(_controller);        
    }
}
