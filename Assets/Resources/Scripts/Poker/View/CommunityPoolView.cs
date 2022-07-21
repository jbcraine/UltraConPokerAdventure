using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CommunityPoolView : MonoBehaviour
{
    [SerializeField]
    private GameObject _CardViewPrefab;
    private List<CardView> _PoolCards;
    private HorizontalLayoutGroup _Layout;
    private int _RevealedCards = 0;
    private void Awake()
    {
        _Layout = GetComponent<HorizontalLayoutGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void HookupToCommunityPool(CommunityPool pool)
    {
        pool.CardsRevealed += RevealCards;
        pool.PoolCleared += ClearCards;
        pool.PoolFilled += SetCardViews;

        //InstantiateCardViews(pool.CardAmount);
    }

    public void SetCardViews(List<Card> cards)
    {
        if (_PoolCards == null || _PoolCards.Count == 0)
            InstantiateCardViews(cards.Count);

        foreach(var c in _PoolCards)
        {
            Debug.Log(c);
        }
        if (cards.Count != _PoolCards.Count)
            return;

        for (int i = 0; i < _PoolCards.Count; i++)
        {
            _PoolCards[i].SetFace(cards[i].face, cards[i].suit);
        }

        HideCards();
    }

    //Create a CardView for each hand card
    private void InstantiateCardViews(int amount)
    {
        _PoolCards = new List<CardView>();
        for (int i = 0; i < amount; i++)
        {
            GameObject cardview = Instantiate(_CardViewPrefab, transform, false);
            cardview.GetComponent<RectTransform>().localScale = Vector2.one;
            _PoolCards.Add(cardview.GetComponent<CardView>());
            //cardview.transform.SetParent(gameObject.transform, false);
            //cardview.transform.localScale = Vector3.one;
            //cardview.transform.position = new Vector3(cardview.transform.position.x, cardview.transform.position.y, -1);
        }
    }

    private void RevealCards(int amount)
    {
        if (_RevealedCards >= _PoolCards.Count)
            return;

        amount = Math.Min(amount, _PoolCards.Count - _RevealedCards);
        for (; _RevealedCards < _RevealedCards + amount; _RevealedCards++)
        {
            _PoolCards[_RevealedCards].SetFaceUp(true);
        }
    }

    private void HideCards()
    {
        _PoolCards.ForEach(card => card.SetFaceUp(false));
    }

    private void ClearCards()
    {
        foreach(CardView card in _PoolCards)
        {
            card.SetFaceUp(false);
        }
    }
}
