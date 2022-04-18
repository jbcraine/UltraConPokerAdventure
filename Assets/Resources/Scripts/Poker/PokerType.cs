using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum InitialBetType
{
    None = 0,
    Single_Blinds = 1,
    Double_Blinds = 2,
    Ante = 3
}

//Each variant of Poker inherits from PokerType
public abstract class PokerType
{
    //INSTANCE PROPERTIES
    //The minimum each contestant needs to start. Contestants may exceed this amount at the start of a game.
    public int MinimumStartingMoney;
    public int StartingBet;
    public int MaxBet;
    private List<Contestant> _Contestants;
    private int _MainPot = 0;
    private int _Round = 0;
    private int _Phase = 0;
    private int _CurrentBet = 0;
    private int _CurrentContestantIndex;

    //CLASS PROPERTIES
    public readonly int MaxPhases;
    public readonly int RoundsToRaiseStartingBet;
    public readonly int CardsPerContestant;
    private HandScorer Scorer;
    private HandAnalyzer Analyzer;
    private List<SidePot> SidePots;

    //GETTERS
    public int MainPot { get { return _MainPot; } }
    public int Round { get { return _Round; } }
    public int Phase { get { return _Phase; } }
    public int CurrentBet { get { return _CurrentBet; } }
    public int ContestantsRemainingInRound
    {
        get
        {
            int count = 0;
            foreach (var contestant in _Contestants)
                count = contestant.Status.HasFlag(ContestantStatus.Folded | ContestantStatus.Eliminated) ? count : count + 1;
            return count;
        }
    }

    public int ContestantsRemainingInMatch
    {
        get
        {
            int count = 0;
            foreach (var contestant in _Contestants)
                count = contestant.Status.HasFlag(ContestantStatus.Eliminated) ? count : count + 1;
            return count;
        }
    }
    public Contestant CurrentContestant
    {
        get
        {
            return _Contestants[_CurrentContestantIndex];
        }
    }
    //Take a configuration file and set the rules of the game
    public virtual void Initialize()
    {
        Scorer = new HandScorer(Analyzer);
        SidePots = new List<SidePot>();
    }

    public virtual void TakeDecision(PokerCommand decision)
    {
        decision.Execute();
    }

    public void SetContestants(List<Contestant> contestants)
    {
        _Contestants = contestants;
    }

    public virtual void AskContestantForDecision()
    {
        CurrentContestant.MakeDecision(new PokerState(CurrentBet, MainPot, this));
    }

    public virtual void ContestantDecisionMade(PokerCommand decision)
    {
        //Process the decision.
        decision.Execute();
        FindNextContestant();
    }
    public virtual float ScoreHand(Hand hand)
    {
        HandInfo score = Scorer.ScoreHand(hand);
        return score.HandScore;
    }

    public virtual void Call(int money)
    {
        _MainPot += money;
        NextTurn();
    }

    public virtual void Raise(int raisedBet)
    {
        _MainPot += raisedBet - CurrentBet;
        _CurrentBet = raisedBet;
        //Raise
        SetNonCurrentContestantsToWaiting();
        NextTurn();
    }

    public virtual void Fold()
    {
        NextTurn();
    }

    protected void SetNonCurrentContestantsToWaiting()
    {
        int index = _CurrentContestantIndex;
        while (++index != _CurrentContestantIndex)
        {
            if (index >= _Contestants.Count)
                index = 0;

            //Contestants not participating do not get updated
            if (!(_Contestants[index].HasStatus(ContestantStatus.Folded | ContestantStatus.Eliminated)))
                _Contestants[index].Status = ContestantStatus.Waiting;
        }
    }

    public List<Contestant> DetermineWinner(List<Contestant> contestants)
    {
        return null;
    }

    public virtual void NextTurn()
    {
        int nowContestant = _CurrentContestantIndex;
        FindNextContestant();
        if (nowContestant == _CurrentContestantIndex)
        {
            //Activate next phase
        }
    }

    public virtual void NextPhase()
    {
        _Phase++;
    }

    protected virtual void SetNextPhase()
    {

    }

    public virtual void NextRound()
    {
        _Round++;
    }

    protected virtual void SetNextRound()
    {
        _Phase = 0;
    }

    //A valid next contestant is one that is waiting to make a decision.
    protected virtual void FindNextContestant()
    {
        //Find the first contestant that is waiting
        int startIndex = _CurrentContestantIndex;
        while ((_Contestants[_CurrentContestantIndex].Status & ContestantStatus.Waiting) != ContestantStatus.Waiting)
        {
            _CurrentContestantIndex = (_CurrentContestantIndex + 1) > _Contestants.Count ? 0 : _CurrentContestantIndex + 1;

            //If a cycle is detected, then no contestants are waiting and the phase may end
            if (startIndex == _CurrentContestantIndex)
            {
                return;
            }
        }


    }


    //Remove a contestant from the round when they fold
    protected virtual void RemoveContestantFromRound(Contestant contestant)
    {
        contestant.Status = ContestantStatus.Folded;
    }

    //Remove a contestant from the game when they run out of money
    protected virtual void RemoveContestantFromMatch(Contestant contestant)
    {
        contestant.Hand.EmptyHand();
        contestant.Status = ContestantStatus.Eliminated;
    }


    //Try to create additional side pots in the event of all-ins
    protected virtual void CreateSidePots()
    {
        //Side pots are only created when more than 3 contestants have reached the showdown phase
        //If there are only two contestants, then see if one of them is entitled to a refund
        if (ContestantsRemainingInRound < 3)
        {
            DetermineRefund(_Contestants);
            return;
        }

        int numAllIn = 0;
        foreach (Contestant contestant in _Contestants)
        {
            if (contestant.allIn)
                ++numAllIn;
        }

        //Side pots are only created if there exists a discrepency between the amounts of the contestants all-ins
        if (numAllIn < 1)
            return;

        List<Contestant> contestantsByAmountBet = SortContestantsFromBet(_Contestants);

        //The main pot is determined by the lowest bet that all contestants have met. Each contestant can claim this pot
        int mainPot;
        int lowBet = contestantsByAmountBet[0].CurrentlyWageredMoney;
        mainPot = lowBet * contestantsByAmountBet.Count;
        foreach (Contestant contestant in contestantsByAmountBet)
        {
            if (contestant.CurrentlyWageredMoney == lowBet)
                contestantsByAmountBet.Remove(contestant);
        }

        //Create as many side pots as necessary (Generally count - 2, potentially fewer if multiple contestants have identical bets)
        //If only one contestant has the highest call, then that contestant gets a refund
        //If multiple contestants have the highest call, then they take part in the highest side pot
        while (contestantsByAmountBet.Count > 1)
        {
            //Bet is the lowest bet among the contestants
            int bet = contestantsByAmountBet[0].CurrentlyWageredMoney;
            int remainder = bet - lowBet;
            int sidePotAmount = remainder * (contestantsByAmountBet.Count);
            _sidePots.Add(new SidePot(contestantsByAmountBet, sidePotAmount));

            //Do not consider any contestant who has wagered up to current bet for any future side pots
            foreach (Contestant contestant in contestantsByAmountBet)
            {
                if (contestant.CurrentlyWageredMoney == bet)
                    contestantsByAmountBet.Remove(contestant);
            }

            //Use the previous bet to determine remainders for the next sidePot
            lowBet = bet;
        }

        //The highest betting contestant is entitled to a refund of however much money no other contestant could match in their bet
        if (contestantsByAmountBet.Count == 1)
        {
            RefundContestant(contestantsByAmountBet[0], lowBet);
        }
    }

    protected virtual void DetermineRefund(List<Contestant> contestants)
    {
        Contestant highestBetter = _Contestants[0];
        if (_Contestants[1].CurrentlyWageredMoney > highestBetter.CurrentlyWageredMoney)
        {
            highestBetter = _Contestants[1];
            RefundContestant(highestBetter, _Contestants[0].CurrentlyWageredMoney);
        }
        else if (_Contestants[1].CurrentlyWageredMoney < highestBetter.CurrentlyWageredMoney)
        {
            RefundContestant(highestBetter, _Contestants[1].CurrentlyWageredMoney);
        }
        //Else, if the contstants bet equally, do nothing
        else
        {
            return;
        }
    }

    public virtual void Reset()
    {
        _Round = 0;
        _Phase = 0;
        _MainPot = 0;
        _CurrentBet = 0;
    }
}
