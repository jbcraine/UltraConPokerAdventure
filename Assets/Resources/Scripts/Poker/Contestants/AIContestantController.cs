using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIContestantController : ContestantController
{
    [SerializeField] private PokerAI _AI;
    protected bool[] knownCards;

    private void Awake()
    {
        _AI = GetComponent<PokerAI>();
        knownCards = new bool[52];
    }

    public override void Initialize(HandScorer scorer, ContestantModel model)
    {
        _model = model;
        _handScorer = scorer;
        _AI.Initialize(scorer, this);
    }

    public override void MakeDecision(PokerState state)
    {
        long decision = _AI.MakeDecision(state, _model, state.Game.ContestantsRemainingInRound, knownCards);
        if (decision == -1)
        {
            DecisionMade(Fold(state));
        }
        else if (decision == 0)
        {
            DecisionMade(Call(state));
        }
        else if (decision == state.CurrentBet)
        {
            DecisionMade(Call(state));
        }
        else
        {
            DecisionMade(Raise(decision, state));
        }
    }

    private void ResetCards()
    {
        for (int i = 0; i < knownCards.Length; i++)
        {
            knownCards[i] = false;
        }
    }
    protected virtual void EvaluateHand()
    {

    }

    protected void AddCardToKnownCards(Card card)
    {
        int index = (int)card.suit * 13 + (int)card.face;
        knownCards[index] = true;
    }

    protected void AddCardsToKnownCards(List<Card> cards)
    {
        cards.ForEach(card => AddCardToKnownCards(card));
    }
}