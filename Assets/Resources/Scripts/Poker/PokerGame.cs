using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerGame : MonoBehaviour
{
    [SerializeField] private Deck _deck;
    [SerializeField] private List<Contestant> _contestants;
    private List<Contestant> _contestantsInMatch;
    private List<Card> _communityCards;
    private List<Contestant> _contestantsInRound;
    private PokerType pokerTypeInstance;

    //The contestant whose turn it is
    private Contestant _currentContestant;

    private bool startingContestantEliminated;
    private int _currentContestantIndex;
    private int _startingContestantIndex;
    public event BetChangedEventHandler BetChanged;
    public event EndRoundEventHandler RoundEnded;
    public event PotChangedEventHandler PotChanged;
    public event RoundStartedEventHandler RoundStarted;
    public event ContestantEliminatedEventHandler ContestantEliminated;
    public event WonPotEventHandler PotWon;
    public event AllContestantsCalledHandler CalledAllContestants;
    public event MatchEndEventHandler MatchEnded;


    public int CurrentBet
    {
        get { return pokerTypeInstance.CurrentBet; }
    }

    public int Pot
    {
        get { return pokerTypeInstance.MainPot; }
    }

    public int Round
    {
        get { return pokerTypeInstance.Round; }
    }

    public int ContestantsRemainingInRound
    {
        get { return _contestantsInRound.Count; }
    }

    public PokerType PokerTypeInstance
    {
        get { return pokerTypeInstance; }
        set { pokerTypeInstance = value; }
    }

    public void Initialize(PokerType pokerTypeInstance)
    {
        PokerTypeInstance = pokerTypeInstance;
    }

    public void StartMatch()
    {
        ResetGame();

        //Give every contestant the starting amount of money and set the max size of their hands
        foreach (Contestant contestant in _contestantsInMatch)
        {
            contestant.ChangeMoney(PokerTypeInstance.MinimumStartingMoney);
            contestant.Hand.SetMaxCards(PokerTypeInstance.CardsPerContestant);
        }

        //Select the initial starting contestant, as well as assigning initial blinds if they are available

        _startingContestantIndex = _currentContestantIndex = Random.Range(0, _contestantsInMatch.Count);
        _currentContestant = _contestantsInMatch[_startingContestantIndex];

        StartNextRound();
    }

    public void StartNextRound()
    {
        
    }

    public void StartNextTurn()
    {

    }


    private void Deal(Contestant contestant)
    {
        List<Card> hand = new List<Card>();
        for (int i = 0; i < contestant.Hand.MaxCards; ++i)
            hand.Add(_deck.PopCard());

        contestant.FillHand(hand);
    }

    private void EndMatch()
    {
        //When the match is over, then the manager can be destroyed
        Contestant winner = _contestantsInMatch[0];
        MatchEnded(this, winner.name);
        Debug.Log("End game");
    }


    private bool AllContestantsCalled()
    {
        foreach (Contestant contestant in _contestantsInRound)
        {
            if (!contestant.HasStatus(ContestantStatus.Called))
                return false;
        }

        return true;
    }

    public void ResetCallStatus()
    {
        foreach (Contestant contestant in _contestantsInRound)
        {
            contestant.Status = ContestantStatus.Waiting;
        }
    }

    //Reset the game state to the starting conditions
    private void ResetGame()
    {
        pokerTypeInstance.Reset();

        _contestantsInMatch = new List<Contestant>(_contestants);

        foreach (Contestant contestant in _contestantsInMatch)
        {
            contestant.ChangeMoney(0);
            contestant.AssignScore(HandInfo.Empty);
            contestant.Reset();
        }
    }
}
