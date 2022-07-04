using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat 
{
    public readonly int MAX_SIZE = 10;
    public int cardCost;
    public List<Card> cards;
    public void AddCard(Card c)
    {
        cards.Add(c);
    }

    public void RemoveCard(CardFace face, CardSuit suit)
    {
        cards.RemoveAll(card => card.face == face && card.suit == suit);
    }

    public void SetCardCost(int newCost)
    {
        cardCost = newCost;
    }

    public List<Card> GetCards()
    {
        return new List<Card>(cards);
    }

    public void SetCards(List<Card> cards)
    {
        this.cards = cards;
    }
}
