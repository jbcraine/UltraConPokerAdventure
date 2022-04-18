using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ContestantDecisionEventHandler(PokerCommand decision);
public enum ContestantStatus
{
    None = 0b0000,
    Waiting = 0b0001,
    Called = 0b0010,
    Folded = 0b0100,
    Eliminated = 0b1000

}
public abstract class Contestant : MonoBehaviour
{
    [SerializeField] protected string _contestantName;
    protected Hand _hand;

    //The money that the contestant has left available to wager in a round
    protected int _money;
    //The money that the contestant has already wagered in a round
    protected int _betMoney;
    protected ContestantStatus _status;
    protected HandInfo _handInfo;
    public ContestantDecisionEventHandler DecisionMade;

    public int Money
    {
        get { return _money; }
        set { _money = value; }
    }

    public int CurrentlyWageredMoney
    {
        get { return _betMoney; }
        set { _betMoney = value; }
    }

    public int TotalMoney
    {
        get { return Money + CurrentlyWageredMoney; }
    }

    public Hand Hand
    {
        get { return _hand; }
    }

    public ContestantStatus Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public bool AllIn
    {
        get { return Money == 0; }
    }

    public string BestHandName
    {
        get { return _handInfo.HandName; }
    }

    public float BestHandScore
    {
        get { return _handInfo.HandScore; }
    }

    public int[] BestHand
    {
        get { return _handInfo.Cards; }
    }

    public virtual void Initialize()
    {
        
    }
    public void FillHand(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            //Debug.Log(card.value + " of " + card.suit);
            _hand.AddCard(card);
        }
    }

    protected virtual void Raise(int raisedBet, PokerState state)
    {
        //Make sure the raisedBet is greater than the currentBet
        if (raisedBet <= state.CurrentBet)
        {
            Call(state);
            return;
        }
        //Make sure the contestant cannot bet more money than then have
        raisedBet = raisedBet >= TotalMoney ? TotalMoney : raisedBet;

        int contribution = raisedBet - _betMoney;

        RaiseCommand raise = new RaiseCommand(new RaiseCommandArgs(this, raisedBet, state.Game));
        DecisionMade(raise);
        ChangeMoney(-contribution);
        _betMoney += contribution;
        Status = ContestantStatus.Called;
    }
   
    protected virtual void Call(PokerState state)
    {
        int contribution = state.CurrentBet - _betMoney;

        //If the contestant does not have enough money to meet the call, then offer whatever money the contestant has left
        contribution = contribution <= Money ? Money : contribution;

        CallCommand call = new CallCommand(new CallCommandArgs(this, contribution, state.Game));
        DecisionMade(call);
        ChangeMoney(-contribution);
        _betMoney += contribution;
        Status = ContestantStatus.Called;
    }

    protected virtual void Check(PokerState state)
    {
        CheckCommand check = new CheckCommand(new CheckCommandArgs(this, state.Game));
        DecisionMade(check);
        Status = ContestantStatus.Called;
    }

    protected virtual void Fold(PokerState state)
    {
        FoldCommand fold = new FoldCommand(new FoldCommandArgs(this, state.Game));
        DecisionMade(fold);
        Status = ContestantStatus.Folded;
    }

    public virtual void ChangeMoney(int money)
    {
        _money += money;
        if (_money < 0)
            _money = 0;
    }

    public void ResetBet()
    {
        _betMoney = 0;
    }

    public void AssignScore(HandInfo hand)
    {
        _handInfo = hand;   
    }
    public virtual void MakeDecision(PokerState gameState)
    {
        //Call one of the decision methods
    }

    public bool HasStatus(ContestantStatus status)
    {
        return (this.Status & status) > 0;
    }

    public void Reset()
    {
        Hand.EmptyHand();
        CurrentlyWageredMoney = 0;
        _handInfo = HandInfo.Empty;
        Status = ContestantStatus.None;
    }
}
