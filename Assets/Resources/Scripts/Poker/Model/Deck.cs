using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private const int STANDARD_DECK_SIZE = 52;
    private const int TOTAL_VALUES = 13;
    private const int TOTAL_SUITS = 4;
    private List<Card> _deck;
    public int DeckRemaining { get { return _deck == null ? 0 : _deck.Count; } }


    public Deck()
    {
        _deck = new List<Card>();
    }
    public Deck (Deck d)
    {
        _deck = new List<Card>(d._deck);
    }

    public List<Card> CloneCards()
    {
        List<Card> cards = new List<Card>();
        foreach (Card c in _deck)
        {
            cards.Add(c);
        }

        return cards;
    }
    public void FillStandardDeck()
    {
        _deck.Clear();

        for (int suit = 0; suit < TOTAL_SUITS; ++suit)
        {
            for (int value = 0; value < TOTAL_VALUES; ++value)
            {
                Card card = new Card((CardFace)value, (CardSuit)suit);
                _deck.Add(card);
            }
        }
    }

    public void SetDeck(List<Card> cards)
    {
        _deck = cards;
    }
    public void RemoveCard(Card c)
    {
        _deck.RemoveAll(card => card.face == c.face && card.suit == c.suit);
    }
    //Remove all cards from the deck with the given value by replacing them with a null value
    public void RemoveCardValue(CardFace value)
    {
        _deck.RemoveAll(CardHasValue);

        bool CardHasValue(Card card)
        {
            return card.face == value;
        }
    }

    //Remove all cards from the deck with the given suit
    public void RemoveCardSuit(CardSuit suit)
    {
        _deck.RemoveAll(CardHasSuit);

        bool CardHasSuit(Card card)
        {
            return card.suit == suit;
        }
    }

    //Change all cards with the given value to have a new value
    public void ChangeCardValue(CardFace value, CardFace newValue)
    {
        _deck.ForEach(CardHasValue);

        void CardHasValue(Card card)
        {
            if (card.face == newValue)
                card.ChangeFace(newValue);
        }
    }

    //Change all cards with the given suit to have a new suit
    public void ChangeCardSuit(CardSuit suit, CardSuit newSuit)
    {
        _deck.ForEach(CardHasSuit);

        void CardHasSuit(Card card)
        {
            if (card.suit == suit)
                card.ChangeSuit(newSuit);
        }
    }

    //Shuffle the deck with a version of the Knute shuffle algorithm, which swaps at each index with another random index 
    public void ShuffleDeck()
    {
        for (int i = 0; i < DeckRemaining; i++)
        {
            Card temp;
            temp = _deck[i];
            int randomIndex = Random.Range(0, DeckRemaining);
            _deck[i] = _deck[randomIndex];
            _deck[randomIndex] = temp;
        }
    }

    //Return the card at the top of the deck and decrease its size
    public Card PopCard()
    {
        Card c = null;
        if (DeckRemaining > 0)
        {
            c = _deck[DeckRemaining - 1];
            _deck.RemoveAt(DeckRemaining - 1);
        }
        return c;
    }

    public List<Card> BuildHand(int handsize)
    {
        List<Card> hand = new List<Card>();

        for (int i = 0; i < handsize; i++)
            hand.Add(PopCard());
        return hand;
    }

}