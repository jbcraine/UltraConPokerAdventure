using UnityEngine;

public class PlayerPoolPokerContestant : PoolPokerContestant
{
    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<PlayerPoolPokerController>();
        Debug.Log(_controller);
    }
}
