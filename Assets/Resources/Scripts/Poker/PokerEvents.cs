using System;
using System.Collections.Generic;

public delegate void WonPotEventHandler(object sender, WonPotEventArgs p);
public delegate void RoundStartedEventHandler(long staSrtingMoney);
public delegate void EndRoundEventHandler(object sender);
public delegate void PotChangedEventHandler(long pot);
public delegate PokerCommand PlayerMadeDecisionHandler(object sender, PokerCommand decision);
public delegate void PhaseStartEventHandler(object sender);
public delegate void MoneyEventHandler(object sender, long money);
public delegate void MatchEndEventHandler(object sender, string winnerName);
public delegate void ContestantEliminatedEventHandler(string contestantName);
public delegate void ClearHandHandler(object sender);
public delegate void CardEventHandler(object sender, CardEventArgs e);
public delegate void BetChangedEventHandler(long currentBet);
public delegate void AllContestantsCalledHandler(object sender);
public delegate void RaiseEventHandler(RaiseDecisionInfo raiseInfo);
public delegate void CallEventHandler(CallDecisionInfo callInfo);
public delegate void CheckEventHandler(CheckDecisionInfo checkInfo);
public delegate void FoldEventHandler(FoldDecisionInfo foldInfo);
public delegate void HandFilledEventHandler(List<Card> cards);
public delegate void ContestantMoneyChangeEventHandler(long newMoney);
public delegate void ContestantBetChangedEventHandler(long newBet);
public delegate void ContestantReceivedHand(List<Card> cards);


public class WonPotEventArgs : EventArgs
{
    public string winnerName { get; private set; }
    public string handName { get; private set; }
    public bool showDownHappened { get; private set; }
    public bool showDownDrawed { get; private set; }

    public WonPotEventArgs(string winnerName, string handName, bool showDownHappened, bool showDownDrawed)
    {
        this.winnerName = winnerName;
        this.handName = handName;
        this.showDownHappened = showDownHappened;
        this.showDownDrawed = showDownDrawed;
    }
}

public class CardEventArgs : EventArgs
{
    public CardFace cardFace { get; private set; }
    public CardSuit cardSuit { get; private set; }

    public CardEventArgs(CardFace value, CardSuit suit)
    {
        cardFace = value;
        cardSuit = suit;
    }
}