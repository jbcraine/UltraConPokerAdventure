using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHand : Hand
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BumpHand(int amount)
    {
        foreach (Card c in cards)
        {
            int newFace = c.FaceValue + amount;
            //TODO: Account for if newFace < 0
            newFace %= 13;
            c.ChangeFace((CardFace)newFace);
        }
    }

    public void ChangeSuits(CardSuit oldSuit, CardSuit newSuit)
    {

    }

    public void ChangeFaces(CardFace[] faces, CardFace newFace)
    {

    }

    public void ChangeFace(CardFace oldFace, CardFace newFace)
    {

    }
}
