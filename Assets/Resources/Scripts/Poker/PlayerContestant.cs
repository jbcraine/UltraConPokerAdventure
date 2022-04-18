using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContestant : Contestant
{
    //Include a event to deactivate some UI features once the player makes a decision

    public event MoneyEventHandler MoneyChanged;
    public event PlayerMadeDecisionHandler PlayerDecided;

    private void Awake()
    {
        
    }

    public override void ChangeMoney(int money)
    {
        Debug.Log("Money: " + money + "+" + _money);
        _money += money;
        if (_money < 0)
            _money = 0;

        //Debug.Log("Money after: " + _money);

        MoneyChanged(this, _money);
    }

    public override void MakeDecision(PokerState state)
    {
        //Activate the player's UI to make a decision
    }
}