using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CommunityPoolCardsRevealedHandler(int amount);
public delegate void CommunityPoolCardsAddedHandler(List<Card> cards);
public delegate void CommunityPoolClearedHandler();
public class CommunityPool : Hand
{
    private List<Card> _PoolCards;
    public List<Card> RevealedCards;

    public CommunityPoolCardsRevealedHandler CardsRevealed;
    public CommunityPoolCardsAddedHandler PoolFilled;
    public CommunityPoolClearedHandler PoolCleared;

    public int PoolSize
    {
        get { return _PoolCards.Count; }
    }
    public int RevealedCardCount
    {
        get { return RevealedCards.Count; }
    }

    public List<Card> Pool
    {
        get { return _PoolCards; }
    }

    public CommunityPool()
    {
        _PoolCards = new List<Card>();
        RevealedCards = new List<Card>();
    }

    public void RevealCards(int n)
    {
        int temp = n;
        while (n > 0 && RevealedCardCount < _PoolCards.Count)
        {
            RevealedCards.Add(_PoolCards[RevealedCardCount]);
            --n;
        }

        CardsRevealed(temp - n);
    }

    public void AddCards(List<Card> cards)
    {
        cards.ForEach(card => AddCard(card));
    }

    public void Clear()
    {
        _PoolCards.Clear();
        RevealedCards.Clear();
    }

    public void FillPool(List<Card> cards)
    {
        cards.ForEach(card => _PoolCards.Add(card));
        PoolFilled.Invoke(_PoolCards);
    }
}
