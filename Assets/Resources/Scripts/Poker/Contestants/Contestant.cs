using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Contestant : MonoBehaviour
{
    [SerializeField] protected string _contestantName;
    protected ContestantModel _model;
    protected ContestantController _controller;
    protected ContestantStatus _status;
    protected HandInfo _handInfo;
    protected HandScorer _handScorer;
    public ContestantDecisionEventHandler DecisionMade;

    public long Money
    {
        get { return _model.Money; }
        set { _model.Money = value; }
    }

    public long CurrentlyWageredMoney
    {
        get { return _model.CurrentlyWageredMoney; }
        set { _model.CurrentlyWageredMoney = value; }
    }

    public long TotalMoney
    {
        get { return Money + CurrentlyWageredMoney; }
    }

    public Hand Hand
    {
        get { return _model.Hand; }
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

    public virtual void DetermineScore()
    {
        _handInfo = _handScorer.ScoreHand(Hand);
    }

    public virtual void MakeDecision(PokerState gameState)
    {
        _controller.MakeDecision(gameState);
    }

    public virtual void FillHand(List<Card> cards)
    {
        _model.FillHand(cards);
    }

    public virtual void ChangeMoney(long money)
    {
        _model.Money += money;
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
