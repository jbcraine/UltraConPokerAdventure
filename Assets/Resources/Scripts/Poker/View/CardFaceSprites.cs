using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "CardFaceSprites", fileName ="CardFaceSprites")]
public class CardFaceSprites : ScriptableObject
{
    public Sprite[] cardFaces;
    public Sprite cardBack;

    public Sprite GetCardSprite(CardFace face, CardSuit suit)
    {
        return cardFaces[(int)suit * 13 + (int)face];
    }
}
