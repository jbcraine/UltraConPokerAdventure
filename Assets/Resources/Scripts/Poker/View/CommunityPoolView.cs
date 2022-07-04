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
    }

    public void SetCardViews(List<Card> cards)
    {
        if (_PoolCards == null || _PoolCards.Count == 0)
            InstantiateCardViews(cards.Count);

        if (cards.Count != _PoolCards.Count)
            return;

        for (int i = 0; i < _PoolCards.Count; i++)
        {
            _PoolCards[i].SetFace(cards[i].face, cards[i].suit);
        }
    }

    //Create a CardView for each hand card
    private void InstantiateCardViews(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject cardview = Instantiate(_CardViewPrefab);
            _PoolCards.Add(cardview.GetComponent<CardView>());
            cardview.transform.parent = this.transform;
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

    private void ClearCards()
    {
        foreach(CardView card in _PoolCards)
        {
            card.SetFaceUp(false);
        }
    }
}
