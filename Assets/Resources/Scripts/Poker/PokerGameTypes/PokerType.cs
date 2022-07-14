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
public class PokerType
{
    //INSTANCE PROPERTIES
    public PokerRuleset Ruleset;
    //The minimum each contestant needs to start. Contestants may exceed this amount at the start of a game.
    public long MinimumStartingMoney;
    public virtual long StartingBet { get; set; }
    protected List<Contestant> _Contestants;
    protected long _MainPot = 0;
    protected int _Round = 0;
    protected long _CurrentBet = 0;
    protected int _CurrentContestantIndex = 0;
    protected Deck _Deck;
    protected HandScorer _Scorer;
    //CLASS PROPERTIES
    protected virtual int RoundsToRaiseStartingBet { get; }
    protected long _cardsPerContestant { get { return Ruleset.maxBet; } }
    private List<SidePot> SidePots;

    //EVENTS
    public event ContestantEliminatedEventHandler ContestantEliminated;
    public event MatchEndEventHandler MatchEnded;

    //GETTERS
    public virtual long MainPot { get { return _MainPot; } }
    public virtual int Round { get { return _Round; } }
    public virtual long CurrentBet { get { return _CurrentBet; } }
    public virtual int CurrentContestantIndex { get { return _CurrentContestantIndex; } }
    public virtual Deck Deck { get { return _Deck; } }
    public int CardsPerContestant { get { return Ruleset.cardsPerContestant; } }
    public virtual Contestant NextContestant { get {
            int nextIndex = CurrentContestantIndex + 1;
            while (nextIndex != CurrentContestantIndex)
            {
                if (nextIndex >= _Contestants.Count)
                {
                    nextIndex = 0;
                    if (nextIndex == CurrentContestantIndex)
                        break;
                }

                if (_Contestants[nextIndex].HasStatus(ContestantStatus.Waiting))
                {
                    _CurrentContestantIndex = nextIndex;
                    return _Contestants[nextIndex];
                }

                nextIndex++;
            }
            return _Contestants[CurrentContestantIndex];
        } }

    public virtual Contestant CurrentContestant { get
        {
            return _Contestants[CurrentContestantIndex];
        }
    }

    public virtual int ContestantsRemainingInRound
    {
        get
        {
            int count = 0;
            foreach (var contestant in _Contestants)
                count = contestant.Status.HasFlag(ContestantStatus.Folded | ContestantStatus.Eliminated) ? count : count + 1;
            return count;
        }
    }

    public virtual int ContestantsRemainingInMatch
    {
        get
        {
            int count = 0;
            foreach (var contestant in _Contestants)
                count = contestant.Status.HasFlag(ContestantStatus.Eliminated) ? count : count + 1;
            return count;
        }
    }

    public virtual bool RoundOver
    {
        //Check if phase==maxphase and all contestants not waiting
        get
        {
            foreach (Contestant c in _Contestants)
            {
                if (c.Status == ContestantStatus.Waiting)
                    return false;
                
            }
            return _Contestants.Count == 1;
        }
    }

    public virtual bool MatchOver
    {
        get
        {
            return _Contestants.Count == 1;
        }
    }

    //Take a configuration file and set the rules of the game
    public virtual void Initialize(PokerGameInitializationInfo info, List<Contestant> contestants)
    {
        if (info != null)
        {
            StartingBet = info.StartingBet;
            MinimumStartingMoney = info.MinimumStartingMoney;
        }

        _Contestants = contestants;
        //Each contestant receives the same HandScorer
        _Scorer = new HandScorer(new HandAnalyzer());
        _Contestants.ForEach(contestant => {
                contestant.Initialize(_Scorer);
            }
        );
        _Deck = new Deck();
        SidePots = new List<SidePot>();
    }

    public virtual void StartMatch()
    {
        System.Random rnd = new System.Random();

        //Choose the starting contestant
        _CurrentContestantIndex = rnd.Next(0, _Contestants.Count - 1);
        //Execute more rounds until the match is over
        while (true)
        {
            NextRound();

            if (MatchOver)
            {
                break;
            }
        }
        EndMatch();
    }

    public virtual void EndMatch()
    {
        MatchEnded?.Invoke(this, _Contestants[0].ContestantName);
    }

    public virtual void NextRound()
    {
        _Round++;
        _Deck.FillStandardDeck();
        _Deck.ShuffleDeck();
        Deal();
        Contestant current = CurrentContestant;
        while (true)
        {
            if (RoundOver)
                break;

            NextTurn(current);
            current = NextContestant;
        }

        EndRound();
    }

    protected virtual void EndRound()
    {
        EliminateContestants();
        _MainPot = 0;
        _CurrentBet = 0;
    }

    public virtual void NextTurn(Contestant contestant)
    {
        //contestant.DecisionMade += ExecuteDecision; //Allow a decision
        AskContestantForDecision(contestant);
        //log decision
        //contestant.DecisionMade -= ExecuteDecision; //Don't allow deciions when not their turn
    }

    public virtual void HandleDecision(PokerCommand decision)
    {
        ExecuteDecision(decision);
    }

    public virtual void EndTurn()
    {
        return;
    }

    public virtual void NextPhase()
    {
        return;
    }

    public virtual void ExecuteDecision(PokerCommand decision)
    {
        decision.Execute();
    }

    public void SetContestants(List<Contestant> contestants)
    {
        _Contestants = contestants;
    }

    //The PokerDecision is returned via a ContestantDecisionEventHandler
        //This is done so that human contestants and AI contestants send back commands in a regular manner
    public virtual void AskContestantForDecision(Contestant contestant)
    {
        contestant.DecisionMade += ExecuteDecision;
        contestant.MakeDecision(new PokerState(CurrentBet, MainPot, this));
    }

    public virtual void Deal()
    {
        _Contestants.ForEach(contestant =>
        {
            contestant.FillHand(_Deck.BuildHand(CardsPerContestant));
        });
    }

    public virtual void Call(long money)
    {
        _MainPot += money;
    }

    public virtual void Raise(long raisedBet)
    {
        _MainPot += raisedBet - CurrentBet;
        _CurrentBet = raisedBet;
        //Raise
        SetNonCurrentContestantsToWaiting();
    }

    public virtual void Fold()
    {
        return;
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

    public virtual List<Contestant> Showdown()
    {
        //Determine the winner of the round
        List<Contestant> winners = new List<Contestant>();
        Contestant winner = null;

        JudgeHands();
        foreach (var contestant in _Contestants)
        {
            if (contestant.InRound)
            {
                if (!winner)
                {
                    winner = contestant;
                    continue;
                }

                if (winner.BestHandScore == contestant.BestHandScore)
                {
                    winners.Add(contestant);
                }
                else if (winner.BestHandScore < contestant.BestHandScore)
                {
                    if (winners.Count > 1)
                        winners.Clear();
                    winners.Add(contestant);
                    winner = contestant;
                }
            }
        }
        return winners;
    }

    protected void JudgeHands()
    {
        foreach (var contestant in _Contestants)
        {
            contestant.DetermineScore();       
        }
    }

    protected void AwardWinners(List<Contestant> winners)
    {
        if (winners.Count == 1) //Single winner
        {
            winners[0].ChangeMoney(MainPot);
        }
        else //Draw. Split the pot
        {
            long splitPot = MainPot / winners.Count;
            winners.ForEach(winner => winner.ChangeMoney(splitPot));
        }
    }

    protected void EliminateContestants()
    {
        _Contestants.ForEach(contestant => {
            if (contestant.Money == 0)
            {
                contestant.Status = ContestantStatus.Eliminated;
                ContestantEliminated?.Invoke(this, contestant.ContestantName);
            }
        });

        _Contestants.RemoveAll(contestant =>
           contestant.Status == ContestantStatus.Eliminated
        );
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

    protected virtual void PhaseCheck()
    {
        return;
    }
    public virtual void Reset()
    {
        _Round = 0;
        _MainPot = 0;
        _CurrentBet = 0;
    }
}
