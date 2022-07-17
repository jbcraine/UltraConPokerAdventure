using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContestantController : ContestantController
{
    //Include a event to deactivate some UI features once the player makes a decision

    public MoneyEventHandler MoneyChanged;
    public PlayerMadeDecisionHandler PlayerDecided;
    public EnableUIEvent UIEnabled;
    public InitiateUIEvent UIInitiated;
    public RaiseEventHandler PlayerRaised;
    public CallEventHandler PlayerCalled;
    public CheckEventHandler PlayerChecked;
    public FoldEventHandler PlayerFolded;
    public HandFilledEventHandler HandFilled;
    private PokerState _recentState;


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
        UnhookEvents();
    }

    private void OnCall(CallDecisionInfo info)
    {
        Call(_recentState);
        UnhookEvents();
    }

    private void OnCheck(CheckDecisionInfo info)
    {
        Check(_recentState);
        UnhookEvents();
    }

    private void OnFold(FoldDecisionInfo info)
    {
        Fold(_recentState);
        UnhookEvents();
    }

    public void HookupUI(PokerUI ui)
    {
        HandFilled += ui.SetPlayerCards;
    }

}