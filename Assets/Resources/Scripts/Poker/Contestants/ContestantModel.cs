using System.Collections.Generic;

public delegate void ContestantDecisionEventHandler(PokerCommand decision);
public enum ContestantStatus
{
    None = 0b0000,
    Waiting = 0b0001,
    Called = 0b0010,
    Folded = 0b0100,
    Eliminated = 0b1000

}
public class ContestantModel
{
    protected Hand _hand;

    //The money that the contestant has left available to wager in a round
    protected long _money;
    //The money that the contestant has already wagered in a round
    protected long _betMoney = 0;

    public ContestantMoneyChangeEventHandler MoneyChanged;
    public ContestantBetChangedEventHandler BetChanged;
    public ContestantReceivedHand HandChanged;

    public long Money
    {
        get { return _money; }
        set { _money = value; MoneyChanged?.Invoke(_money); }
    }

    public long CurrentlyWageredMoney
    {
        get { return _betMoney; }
        set { _betMoney = value; BetChanged?.Invoke(_betMoney); }
    }

    public long TotalMoney
    {
        get { return Money + CurrentlyWageredMoney; }
    }

    public Hand Hand
    {
        get { return _hand; }
    }

    public bool AllIn
    {
        get { return Money == 0; }
    }

    public virtual void FillHand(List<Card> cards)
    {
        if (_hand == null)
            _hand = new Hand(cards);
        else
        {
            foreach (Card card in cards)
            {
                //Debug.Log(card.value + " of " + card.suit);
                _hand.AddCard(card);
            }
        }

        HandChanged?.Invoke(cards);
    }

    public virtual void ChangeMoney(long money)
    {
        _money += money;
        if (_money < 0)
            _money = 0;
    }

    public void ResetBet()
    {
        _betMoney = 0;
    }
}
