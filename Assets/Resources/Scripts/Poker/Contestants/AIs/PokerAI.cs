using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base template for Poker AI
//Each Poker Type will require its own unique AI
public class PokerAI : MonoBehaviour
{
    //How likely is this character to raise?
    //Range [0,1] : 0 - Passive, 1 - Aggressive
    [SerializeField][Range(0f, 1f)]
    protected float aggressiveness;
    //How likely is this character to call large bets?
    //Raange [0,1] : 0 - Unlikely, 1 - Likely
    [SerializeField][Range (0f, 1f)]
    protected float couragessness;
    //How likely is this character to bluff?
    //Range [0,1] : 0 - Never/Rarely, 1 - Often/Always
    [SerializeField][Range(0f, 1f)]
    protected float slyness;
   
    protected AIContestantController _contestant;

    private HandScorer _scorer;


    public void Initialize(HandScorer scorer, AIContestantController contestant)
    {
        _scorer = scorer;
        _contestant = contestant;
    }

    //Use the modifed hand strength to make a decision for this character
    public virtual long MakeDecision(PokerState gamestate, ContestantModel model, int numContestants, bool[] knownCards)
    {
        int opponents = numContestants - 1;

        //Predict the strength of the hand
        float handStrength = HandStrength(BuildHand(knownCards), opponents, 100, knownCards);
        long betAmount = -1;

        //Basic decision making
        //Play decisions are based on the predicted strength of the hand
        if (handStrength > .65)
        {
            //Raise
            float multiplier = (2f * aggressiveness + 2f * handStrength);
            float rng = Random.Range(-1, 1) / 2;
            double product = gamestate.CurrentBet * (multiplier + rng);
            product += gamestate.CurrentBet;
           
            betAmount = (long)product;
            //Round bet amount to multiple of 10
            betAmount -= betAmount % 10;

            if (betAmount > model.Money)
                betAmount = model.Money;
            else if (betAmount < gamestate.CurrentBet)
                betAmount = gamestate.CurrentBet;
        }
        else if (handStrength > .15)
        {
            //Call or check
            betAmount = gamestate.CurrentBet;
            
        }
        //Else, betAmount = -1, Fold

        return betAmount;
    }

    protected virtual float HandStrength(Hand hand, int numOpponents, int n, bool[] knownCards)
    {
        return RunSimulations(n, hand, numOpponents, knownCards);
    }

    protected virtual float RunSimulations(int n, Hand hand, int numOpponents, bool[] knownCards)
    {
        bool[] unknownCards = new bool[52];
        for (int i = 0; i < 52; i++)
        {
            unknownCards[i] = !knownCards[i];
        }

        Deck deck = BuildTempDeck(unknownCards);
        int count = 0;
        for (int i = 0; i < n; i++)
        {
            Deck cloneDeck = new Deck();
            cloneDeck.SetDeck(deck.CloneCards());
            cloneDeck.ShuffleDeck();
            if (Simulation(cloneDeck, hand, numOpponents))
                count++;
        }
        return count/n;
    }

    //Take the currently known cards
    protected bool Simulation(Deck deck, Hand hand, int numOpponents)
    {
        Debug.Log("Hand " + hand.cards.Count);
        List<Hand> opponentHands = CreateOpponentHands(deck, numOpponents);
        return EvaluateScores(hand, opponentHands);
    }

    private List<Hand> CreateOpponentHands(Deck deck, int numOpponents)
    {
        List<Hand> hands = new List<Hand>();
        for (int i = 0; i < numOpponents; i++)
        {
            hands.Add(CreateOpponentHand(deck));
        }

        return hands;
    }
    private Hand CreateOpponentHand(Deck deck)
    {
        Hand tempHand = new Hand();
        tempHand.AddCard(deck.PopCard());
        tempHand.AddCard(deck.PopCard());
        Debug.Log("Opponent " + tempHand.cards.Count);
        return tempHand;
    }


    //Once the hand strength is obtained, the character's personality can be factored into the result as to the decision that they will make 
    protected virtual float CharacterInfluence(float handStrength)
    {
        return 0;
    }



    private Deck BuildTempDeck(bool[] unknownCards)
    {
        List<Card> cards = new List<Card>();
        for (int i = 0; i < 52; i++)
        {
            if (unknownCards[i])
                cards.Add(new Card((CardFace)(i % 13), (CardSuit)(i / 13)));
        }

        Deck tempDeck = new Deck();
        tempDeck.SetDeck(cards);
        Debug.Log("Deck " + tempDeck);
        return tempDeck;
    }


    private bool EvaluateScores(Hand hand, List<Hand> opponents)
    {
        //Evaluate out hand
        HandInfo score = _scorer.ScoreHand(hand);

        //Evaluate opponent hands
        for (int i = 0; i < opponents.Count; i++)
        {
            HandInfo opponentScore = _scorer.ScoreHand(opponents[i]);
            if (opponentScore.HandScore > score.HandScore)
                return false;
        }

        return true;
    }

    private Hand BuildHand(bool[] knownCards)
    {
        Hand hand = new Hand();
        for (int i = 0; i < knownCards.Length; i++)
        {
            if (knownCards[i])
            {
                hand.AddCard(new Card((CardFace)(i % 13), (CardSuit)(i / 13)));
            }
        }
        return hand;   
    }
}