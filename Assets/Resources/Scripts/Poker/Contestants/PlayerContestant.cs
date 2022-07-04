using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContestant : Contestant
{
    //Include a event to deactivate some UI features once the player makes a decision

    public event MoneyEventHandler MoneyChanged;
    public event PlayerMadeDecisionHandler PlayerDecided;
    public event EnableUIEvent UIEnabled;
    public RaiseEventHandler PlayerRaised;
    public CallEventHandler PlayerCalled;
    public CheckEventHandler PlayerChecked;
    public FoldEventHandler PlayerFolded;
    public HandFilledEventHandler HandFilled;
    private PokerState _recentState;

    private void Awake()
    {
        PokerUI ui = FindObjectOfType<PokerUI>();
        HandFilled += ui.SetPlayerCards;
    }

    public override void ChangeMoney(long money)
    {
        Debug.Log("Money: " + money + "+" + _money);
        _money += money;
        if (_money < 0)
            _money = 0;

        //Debug.Log("Money after: " + _money);

        MoneyChanged(this, _money);
    }

    public override void FillHand(List<Card> cards)
    {
        base.FillHand(cards);
        HandFilled?.Invoke(cards);

    }

    public override void MakeDecision(PokerState state)
    {
        _recentState = state;
        //Activate the player's UI to make a decision
        //PokerCommand command = GetPlayerDecision()
        PlayerRaised += OnRaise;
        PlayerCalled += OnCall;
        PlayerChecked += OnCheck;
        PlayerFolded += OnFold;

        UIEnabled?.Invoke(this);

        return;
    }

    private void UnhookEvents()
    {
        PlayerRaised -= OnRaise;
        PlayerCalled -= OnCall;
        PlayerChecked -= OnCheck;
        PlayerFolded -= OnFold;
    }

    private void OnRaise(RaiseDecisionInfo info)
    {
        Raise(info.bet, _recentState);
    }

    private void OnCall(CallDecisionInfo info)
    {
        Call(_recentState);
    }

    private void OnCheck(CheckDecisionInfo info)
    {
        Check(_recentState);
    }

    private void OnFold(FoldDecisionInfo info)
    {
        Fold(_recentState);
    }
}