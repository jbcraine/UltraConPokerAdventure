using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    private const int STANDARD_HAND_SIZE = 5;
    public List<Card> cards { get; private set; }
    private int _currentCards;

    public int CardAmount { get { return _currentCards; } }
    public int MaxCards { get { return cards.Count; } }

    public Hand()
    {
        cards = new List<Card>();
    }

    public Hand(ICollection<Card> cards)
    {
        List<Card> tempHand = new List<Card>();
        foreach(Card card in cards)
        {
            tempHand.Add(card);
        }

        this.cards = tempHand;
    }

    //Construct an amalgam Hand
    public Hand(ICollection<Hand> hands)
    {
        List<Card> tempHand = new List<Card>();
        foreach (Hand hand in hands)
        {
            foreach (Card card in hand.cards)
            {
                tempHand.Add(card);
            }
        }

        cards = tempHand;
    }

    public void EmptyHand()
    {
        for (int i = 0; i < cards.Count; i++)
            cards[i] = null;
    }

    //Use a simple Insertion Sort to sort the cards
    public void SortHand()
    {
        Card key;
        int i, j;
        for (i = 1; i < cards.Count; ++i)
        {
            key = cards[i];
            j = i - 1;

            while (j >= 0 && cards[j].face > key.face)
            {
                cards[j + 1] = cards[j];
                j -= 1;
            }
            cards[j + 1] = key;
        }
    }

    public void AddCard(Card c)
    {
        //if (_currentCards >= MaxCards)
        //    return;

        cards.Add(c);
    }

}
