using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PokerUI : MonoBehaviour
{
    public InitiateUIEvent UIInitiated;

    public Button RaiseButton;
    public Button CallButton;
    public Button FoldButton;
    public Slider RaiseSlider;
    public TMP_Text PotDisplay;
    public TMP_Text BetDisplay;
    public TMP_Text RaiseDisplay;
    public long CurrentBet;
    public long RaiseValue;
    [SerializeField]
    private HandView _HandView;
    [SerializeField]
    private CommunityPoolView _PoolView;

    

    public void EnableUI(PlayerContestantController player)
    {
        RaiseButton.onClick.AddListener(() => { player.PlayerRaised?.Invoke(new RaiseDecisionInfo(RaiseValue)); DisableUI(); });
        CallButton.onClick.AddListener(() => { player.PlayerCalled?.Invoke(new CallDecisionInfo(CurrentBet)); DisableUI(); });
        FoldButton.onClick.AddListener(() => { player.PlayerFolded?.Invoke(new FoldDecisionInfo()); DisableUI(); });
        SetButtons(true);
        SetSliderValues(CurrentBet, player.Money);
        RaiseSlider.interactable = true;
    }

    private void SetSliderValues(long min, long max)
    {
        RaiseSlider.minValue = min;
        RaiseSlider.maxValue = max;
    }

    public void DisableUI()
    {
        RaiseButton.onClick.RemoveAllListeners();
        CallButton.onClick.RemoveAllListeners();
        FoldButton.onClick.RemoveAllListeners();
        SetButtons(false);
        RaiseSlider.interactable = false;
        RaiseSlider.value = RaiseSlider.minValue;
    }

    public void ChangePotText(long pot)
    {
        PotDisplay.text = "$" + string.Format("{0}", pot);
    }

    public void ChangeRaiseText(long raise)
    {
        RaiseValue = raise;
        RaiseDisplay.text = "$" + string.Format("{0}", raise);
    }

    public void ChangeBetDisplay(long bet)
    {
        CurrentBet = bet;
        BetDisplay.text = "$" + string.Format("{0}", bet);
    }

    public void SetPlayerCards(List<Card> cards)
    {
        _HandView.SetCardViews(cards);
    }

    public void SetPoolCards(List<Card> cards)
    {
        _PoolView.SetCardViews(cards);
    }

    public void HookupToPokerType(PokerType poker)
    {
        if (poker == null)
            return;


        if (poker is CommunityPoolType)
        {
            Debug.Log("Success!");
            var temp = (CommunityPoolType) poker;
           _PoolView.HookupToCommunityPool(temp.Pool);
            Debug.Log(temp.Pool);
        }

        poker.BetChanged += ChangeBetDisplay;
        poker.PotChanged += ChangePotText;
    }

    private void SetButtons(bool active)
    {
        RaiseButton.interactable = active;
        CallButton.interactable = active;
        FoldButton.interactable = active;
    }
}
