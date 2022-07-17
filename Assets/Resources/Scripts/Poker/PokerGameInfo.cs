

public abstract class PokerGameInfo 
{
    public const string CONPOKER = "CONPOKER";
    public const string DRAWPOKER = "DRAWPOKER";
    public const string POOLPOKER = "POOLPOKER";
    public const string STUDPOKER = "STUDPOKER";

    public short contestantCardAmount;
    public bool clockwise;
    public ulong entryMoney;
    public ulong startingBet;
}
