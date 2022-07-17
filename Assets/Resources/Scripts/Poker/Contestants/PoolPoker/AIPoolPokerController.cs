public class AIPoolPokerController : AIContestantController
{
    public PoolPokerContestantModel Model { get { return (PoolPokerContestantModel) _model; } }

    public void Initialize(HandScorer scorer, PoolPokerContestantModel model)
    {
        base.Initialize(scorer, model);

        Model.Pool.CardsRevealed += AddRevealedPoolCardsToKnownCards;
    }
    public void AddRevealedPoolCardsToKnownCards(int numCardsRevealed)
    {
        foreach(Card card in Model.Pool.RevealedCards)
        {
            knownCards[card.id] = true;
        }
    }
}
