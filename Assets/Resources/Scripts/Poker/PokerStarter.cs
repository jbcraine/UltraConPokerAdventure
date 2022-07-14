using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerStarter : MonoBehaviour
{
    [SerializeField]
    PokerGameInitializationInfo info;
    [SerializeField]
    List<Contestant> contestants;
    [SerializeField]
    GameObject PokerUI;
    PokerType PokerGame;

    public void BeginInstance()
    {
        if (info.GameType == PokerGameEnum.TexasHoldEm)
        {
            GameObject obj = Instantiate(PokerUI);
            PokerUI ui = obj.GetComponent<PokerUI>();
            PokerGame = new CommunityPoolType(PokerRulesets.Texas_Holdem);
            PokerGame.Initialize(info, contestants);
            ui.HookupToPokerType(PokerGame);
            PokerGame.StartMatch();
        }
    }
}
