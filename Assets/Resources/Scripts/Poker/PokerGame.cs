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


    #region EndRound
    //Round ends when either all but one contestant folds, or a winning hand is determined
    private void EndRound()
    {
        StartCoroutine("RewardPots");

    }

    //Use an IEnumerator so as to time the revealing of UI elements saying who one, and with what type of hand
    //JUST CALL THE METHOD INSIDE OF THE POKERGAMEINSTANCE
    IEnumerator RewardPots()
    {

        //If more than one contestant remains in the final round, then a showdown decides a winner or draw and the pot is distributed
        if (_contestantsInRound.Count > 1)
        {
            //Check for any all-ins, and create side pots accordingly
            CreateSidePots();

            List<Contestant> roundWinners = PokerTypeInstance.DetermineWinner(_contestantsInRound);
            if (roundWinners.Count == 1)
            {
                Contestant winner = roundWinners[0];

                //Send a message to the UI to display a message saying who won and with what kind of hand
                PotWon(this, new WonPotEventArgs(winner.name, winner.BestHandName, true, false));
                yield return new WaitForSeconds(3);
                RewardMainPot(winner: roundWinners[0]);
            }
            else if (roundWinners.Count > 1)
            {
                string handName = roundWinners[0].BestHandName;
                PotWon(this, new WonPotEventArgs("Draw", handName, true, true));
                RewardMainPot(drawedWinners: roundWinners);
            }
            //If there are any sidepots, then award them out appropriately
            //if (_sidePots.Count > 0)
            //    RewardSidePots();
        }
        //If all other contestants folded, then the last remaining contestant wins the round by default
        else if (_contestantsInRound.Count == 1)
        {
            PotWon(this, new WonPotEventArgs(_currentContestant.name, null, false, false));
            RewardMainPot(winner: _currentContestant);
        }

        StartCoroutine("EliminateContestants");
    }

    //Use an IEnumerator so as to time the revealing of UI elements saying who has been eliminated
    IEnumerator EliminateContestants()
    {
        startingContestantEliminated = false;
        //If a contestant has run out of money, then they are eliminated from play
        foreach (Contestant contestant in _contestantsInRound)
        {
            if (contestant.Money == 0)
            {
                //If the starting contestant was eliminated, then immediately set the starting contestant to the next contestant, and raise a flag
                if (_contestantsInRound[_startingContestantIndex].Equals(contestant))
                {
                    PokerTypeInstance.NextTurn();

                    startingContestantEliminated = true;
                }
                ContestantEliminated(this, contestant.name);
                yield return new WaitForSeconds(3);
                RemoveContestantFromMatch(contestant);
            }
        }

        ClearRound();
    }

    //Clear the slate to prepare for the next round
    void ClearRound()
    {
        //Clear each contestant's hand at the end of every round.
        //TODO: Make sure contestants that were removed from the match also have their hands cleared
        foreach (Contestant contestant in _contestantsInMatch)
        {
            contestant.Reset();
        }

        if (_contestantsInMatch.Count == 1)
        {
            EndMatch();
        }
        else
        {
            //Update the starting contestant before beginning the next round
            if (!startingContestantEliminated)
                _startingContestantIndex = PokerTypeInstance.NextContestant(_startingContestantIndex, _contestantsInMatch.Count);
        }

        RoundEnded(this);
    }
    #endregion

    //Use a list, because, in the case of a draw, there can be considered multiple winners
    private void RewardMainPot(Contestant winner = null, List<Contestant> drawedWinners = null)
    {
        if (winner == null && drawedWinners == null)
        {
            Debug.Log("BIG MISTAKE");
        }
        else if (winner != null && drawedWinners != null)
        {
            Debug.Log("BIGGER MISTAKE");
        }
        else if (drawedWinners != null)
        {
            int portion = Pot / drawedWinners.Count;
            foreach (Contestant drawedContestant in drawedWinners)
            {
                drawedContestant.ChangeMoney(portion);
            }
        }
        else if (winner != null)
        {
            winner.ChangeMoney(Pot);
        }

        _mainPot = 0;
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


    #region SidePots
    //CONDITION: The provided list should only have two entries
    //Provide a refund to whichever contestant made the higher bet
    //REFUND = higherBet - lowerBet
    private void DetermineRefund(List<Contestant> contestants)
    {
        Contestant highestBetter = _contestantsInRound[0];
        if (_contestantsInRound[1].betMoney > highestBetter.betMoney)
        {
            highestBetter = _contestantsInRound[1];
            RefundContestant(highestBetter, _contestantsInRound[0].betMoney);
        }
        else if (_contestantsInRound[1].betMoney < highestBetter.betMoney)
        {
            RefundContestant(highestBetter, _contestantsInRound[1].betMoney);
        }
        //Else, if the contstants bet equally, do nothing
        else
        {
            return;
        }
    }

    //If multiple contestants face showdown, but not all could call the current bet, 
    //then refund the highest better the difference of their bet and the next greatest bet
    private void RefundContestant(Contestant highestBetter, int nextHighestBet)
    {
        if (contestantsRemainingInRound < 2)
            return;

        //Determine the amount to be refunded, and adjust the highestBettingContest's money accordingly
        int refund = highestBetter.betMoney - nextHighestBet;
        highestBetter.ChangeMoney(refund);
    }

    //Try to create additional side pots in the event of all-ins
    private void CreateSidePots()
    {
        //Side pots are only created when more than 3 contestants have reached the showdown phase
        //If there are only two contestants, then see if one of them is entitled to a refund
        if (contestantsRemainingInRound < 3)
        {
            DetermineRefund(_contestantsInRound);
            return;
        }

        int numAllIn = 0;
        foreach (Contestant contestant in _contestantsInRound)
        {
            if (contestant.allIn)
                ++numAllIn;
        }

        //Side pots are only created if there exists a discrepency between the amounts of the contestants all-ins
        if (numAllIn < 1)
            return;

        List<Contestant> contestantsByAmountBet = SortContestantsFromBet(_contestantsInRound);

        //The main pot is determined by the lowest bet that all contestants have met. Each contestant can claim this pot
        int mainPot;
        int lowBet = contestantsByAmountBet[0].betMoney;
        mainPot = lowBet * contestantsByAmountBet.Count;
        foreach (Contestant contestant in contestantsByAmountBet)
        {
            if (contestant.betMoney == lowBet)
                contestantsByAmountBet.Remove(contestant);
        }

        //Create as many side pots as necessary (Generally count - 2, potentially fewer if multiple contestants have identical bets)
        //If only one contestant has the highest call, then that contestant gets a refund
        //If multiple contestants have the highest call, then they take part in the highest side pot
        while (contestantsByAmountBet.Count > 1)
        {
            //Bet is the lowest bet among the contestants
            int bet = contestantsByAmountBet[0].betMoney;
            int remainder = bet - lowBet;
            int sidePotAmount = remainder * (contestantsByAmountBet.Count);
            _sidePots.Add(new SidePot(contestantsByAmountBet, sidePotAmount));

            //Do not consider any contestant who has wagered up to current bet for any future side pots
            foreach (Contestant contestant in contestantsByAmountBet)
            {
                if (contestant.betMoney == bet)
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

    private List<Contestant> SortContestantsFromBet(List<Contestant> contestants)
    {
        if (contestants.Count == 0)
        {
            return contestants;
        }

        //Sort the contestants by the amount they have bet. This will be used to adjust the main pot and create side pots
        //This is done using a basic insertion sort
        List<Contestant> contestantsByAmountBet = new List<Contestant>(_contestantsInRound);
        int n = contestantsByAmountBet.Count;

        for (int i = 1; i < n; ++i)
        {
            Contestant key = contestantsByAmountBet[i];
            int j = i - 1;

            while (j >= 0 && contestantsByAmountBet[j].betMoney > key.betMoney)
            {
                contestantsByAmountBet[j + 1] = contestantsByAmountBet[j];
                j = j - 1;
            }
            contestantsByAmountBet[j + 1] = key;
        }

        return contestantsByAmountBet;
    }
    #endregion

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
