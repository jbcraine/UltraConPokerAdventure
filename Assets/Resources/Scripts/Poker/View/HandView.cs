using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandView : MonoBehaviour
{
    public GameObject CardViewPrefab;
    [SerializeField]
    private List<CardView> _HandCards;
    [SerializeField]
    private int RandomTilt = 0;
    private HorizontalLayoutGroup layout;


    private void Awake()
    {
        layout = GetComponent<HorizontalLayoutGroup>();
    }

    private void TiltCards()
    {
        //TODO
        //Rotate the rect transforms of the CardViews using [-RandomTilt, RandomTilt] as a range
    }

    private IEnumerator CardPopup()
    {
        foreach (CardView card in _HandCards)
        {
            //Animate card moving up
            yield return new WaitForSeconds(0.5f);
        }
    }

    //Push the cards down off of the screen
    private void CardPushDown()
    {
        //Change transforms of the cards
    }
    public void SetCardViews(List<Card> cards)
    {
        if (_HandCards == null || _HandCards.Count == 0)
            InstantiateCardViews(cards.Count);

        if (cards.Count != _HandCards.Count)
            return;

        for (int i = 0; i < _HandCards.Count; i++)
        {
            _HandCards[i].SetFace(cards[i].face, cards[i].suit);
        }
    }

    //Create a CardView for each hand card
    private void InstantiateCardViews(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject cardview = Instantiate(CardViewPrefab);
            _HandCards.Add(cardview.GetComponent<CardView>());
            cardview.transform.parent = this.transform;
        }
    }
}
