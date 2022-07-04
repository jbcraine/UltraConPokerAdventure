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
    protected long _money;
    //The money that the contestant has already wagered in a round
    protected long _betMoney;
    protected ContestantStatus _status;
    protected HandInfo _handInfo;
    protected HandScorer _handScorer;
    public ContestantDecisionEventHandler DecisionMade;

    public long Money
    {
        get { return _money; }
        set { _money = value; }
    }

    public long CurrentlyWageredMoney
    {
        get { return _betMoney; }
        set { _betMoney = value; }
    }

    public long TotalMoney
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

    public string ContestantName
    {
        get { return _contestantName; }
    }

    public bool InRound
    {
        get { return !HasStatus(ContestantStatus.Folded | ContestantStatus.Eliminated); }
    }

    public bool Eliminated
    {
        get { return HasStatus(ContestantStatus.Eliminated); }
    }

    public bool Folded
    {
        get { return HasStatus(ContestantStatus.Folded); }
    }

    public virtual void Initialize(HandScorer scorer)
    {
        _handScorer = scorer;
    }
    public virtual void FillHand(List<Card> cards)
    {
        if (_hand == null)
            _hand = new Hand(cards);
        else
        {
            foreach (Card card in cards)
            {
                //Debug.Log(card.value + " of " + card.suit);
                _hand.AddCard(card);
            }
        }
    }

    protected virtual PokerCommand Raise(long raisedBet, PokerState state)
    {
        //Make sure the raisedBet is greater than the currentBet
        if (raisedBet <= state.CurrentBet)
        {
            return Call(state);
        }
        //Make sure the contestant cannot bet more money than then have
        raisedBet = raisedBet >= TotalMoney ? TotalMoney : raisedBet;

        long contribution = raisedBet - _betMoney;

        RaiseCommand raise = new RaiseCommand(new RaiseCommandArgs(this, raisedBet, state.Game));
        DecisionMade(raise);
        ChangeMoney(-contribution);
        _betMoney += contribution;
        Status = ContestantStatus.Called;
        return raise;
    }
   
    protected virtual PokerCommand Call(PokerState state)
    {
        long contribution = state.CurrentBet - _betMoney;
        if (contribution == 0)
            return Check(state);

        //If the contestant does not have enough money to meet the call, then offer whatever money the contestant has left
        contribution = contribution <= Money ? Money : contribution;

        CallCommand call = new CallCommand(new CallCommandArgs(this, contribution, state.Game));
        DecisionMade(call);
        ChangeMoney(-contribution);
        _betMoney += contribution;
        Status = ContestantStatus.Called;
        return call;
    }

    protected virtual PokerCommand Check(PokerState state)
    {
        CheckCommand check = new CheckCommand(new CheckCommandArgs(this, state.Game));
        DecisionMade(check);
        Status = ContestantStatus.Called;
        return check;
    }

    protected virtual PokerCommand Fold(PokerState state)
    {
        FoldCommand fold = new FoldCommand(new FoldCommandArgs(this, state.Game));
        DecisionMade(fold);
        Status = ContestantStatus.Folded;
        return fold;
    }

    public virtual void ChangeMoney(long money)
    {
        _money += money;
        if (_money < 0)
            _money = 0;
    }

    public void ResetBet()
    {
        _betMoney = 0;
    }

    public virtual void DetermineScore()
    {
        _handInfo = _handScorer.ScoreHand(_hand);
    }

    public virtual void MakeDecision(PokerState gameState)
    {
        //DecisionMade();
        return;
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
