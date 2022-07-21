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
    private float RandomTilt = 0;
    private HorizontalLayoutGroup layout;

    private float RandomTiltAmount { get { return Random.Range(-RandomTilt, RandomTilt); } }

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

        //if (cards.Count != _HandCards.Count)
        //    return;

        for (int i = 0; i < _HandCards.Count; i++)
        {
            _HandCards[i].SetFace(cards[i].face, cards[i].suit);
            _HandCards[i].SetFaceUp(true);
        }

        _HandCards.ForEach(card => {
            card.transform.Rotate(new Vector3(0, 0, RandomTiltAmount));
        });
    }


    //Create a CardView for each hand card
    private void InstantiateCardViews(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject cardview = Instantiate(CardViewPrefab,transform,false);
            _HandCards.Add(cardview.GetComponent<CardView>());
            cardview.GetComponent<RectTransform>().localScale = Vector2.one;
            //cardview.transform.SetParent(this.gameObject.transform, false);
            //cardview.transform.localScale = Vector3.one;
        }
    }
}
