using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Card
{
    public CardFace face { get; private set; }
    public CardSuit suit { get; private set; }

    public int FaceValue
    {
        get { return (int)face; }
    }

    public Card (CardFace face, CardSuit suit)
    {
        this.face = face;
        this.suit = suit;
    }

    public void ChangeFace (CardFace newFace)
    {
        this.face = newFace;
    }

    public void ChangeSuit (CardSuit newSuit)
    {
        this.suit = newSuit;
    }
}
