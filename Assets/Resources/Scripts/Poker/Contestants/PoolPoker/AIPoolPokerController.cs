using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPoolPokerController : AIContestantController
{
    new protected PoolPokerContestantModel _model;

    public void Initialize(HandScorer scorer, PoolPokerContestantModel model)
    {
        base.Initialize(scorer, model);

        _model.Pool.CardsRevealed += AddRevealedPoolCardsToKnownCards;
    }
    public void AddRevealedPoolCardsToKnownCards(int numCardsRevealed)
    {
        foreach(Card card in _model.Pool.RevealedCards)
        {
            knownCards[card.id] = true;
        }
    }
}
