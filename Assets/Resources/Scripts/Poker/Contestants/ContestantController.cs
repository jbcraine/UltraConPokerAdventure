using UnityEngine;

public class ContestantController : MonoBehaviour
{
    protected ContestantModel _model;
    protected ContestantStatus _status;
    protected HandInfo _handInfo;
    protected HandScorer _handScorer;
    public ContestantDecisionEventHandler DecisionMade;

    public ContestantStatus Status
    {
        get { return _status; }
        set { _status = value; }
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

    public long Money
    {
        get { return _model.Money; }
    }
    public bool HasStatus(ContestantStatus status)
    {
        return (this.Status & status) > 0;
    }

    public virtual void MakeDecision(PokerState gameState)
    {
        //DecisionMade();
        return;
    }

    public virtual void Initialize(HandScorer scorer, ContestantModel model)
    {
        _handScorer = scorer;
        _model = model;
    }

    protected virtual PokerCommand Raise(long raisedBet, PokerState state)
    {
        //Make sure the raisedBet is greater than the currentBet
        if (raisedBet <= state.CurrentBet)
        {
            return Call(state);
        }
        //Make sure the contestant cannot bet more money than then have
        raisedBet = raisedBet >= _model.TotalMoney ? _model.TotalMoney : raisedBet;

        long contribution = raisedBet - _model.CurrentlyWageredMoney;

        RaiseCommand raise = new RaiseCommand(new RaiseCommandArgs(raisedBet, state.Game));
        DecisionMade(raise);
        _model.ChangeMoney(-contribution);
        _model.CurrentlyWageredMoney += contribution;
        Status = ContestantStatus.Called;
        return raise;
    }

    protected virtual PokerCommand Call(PokerState state)
    {
        long contribution = state.CurrentBet - _model.CurrentlyWageredMoney;
        if (contribution == 0)
            return Check(state);

        //If the contestant does not have enough money to meet the call, then offer whatever money the contestant has left
        contribution = contribution <= _model.Money ? _model.Money : contribution;

        CallCommand call = new CallCommand(new CallCommandArgs(contribution, state.Game));
        DecisionMade(call);
        _model.ChangeMoney(-contribution);
        _model.CurrentlyWageredMoney += contribution;
        Status = ContestantStatus.Called;
        return call;
    }

    protected virtual PokerCommand Check(PokerState state)
    {
        CheckCommand check = new CheckCommand(new CheckCommandArgs(state.Game));
        DecisionMade(check);
        Status = ContestantStatus.Called;
        return check;
    }

    protected virtual PokerCommand Fold(PokerState state)
    {
        FoldCommand fold = new FoldCommand(new FoldCommandArgs(state.Game));
        DecisionMade(fold);
        Status = ContestantStatus.Folded;
        return fold;
    }
}
