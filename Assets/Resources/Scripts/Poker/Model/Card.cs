using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Card
{
    public CardFace face { get; private set; }
    public CardSuit suit { get; private set; }
    public int id { get { return (int)suit * 13 + (int)face; } }
    public bool revealed;

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

    public void ToggleRevealed()
    {
        revealed = !revealed;
    }

    public void SetRevealed(bool set)
    {
        revealed = set;
    }

    public void ChangeSuit (CardSuit newSuit)
    {
        this.suit = newSuit;
    }
}
