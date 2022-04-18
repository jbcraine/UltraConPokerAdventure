using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "CardFaceSprites", fileName ="CardFaceSprites")]
public class CardFaceSprites : ScriptableObject
{
    public Sprite[] faces;
    public Sprite cardBack;
}
