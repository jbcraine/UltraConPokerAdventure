using System.Collections;
using System.Collections.Generic;

public class SidePot
{
    private List<Contestant> _applicableContestants;
    private int _sidePotAmount;

    public SidePot(List<Contestant> applicalbleContestants, int sidePotAMount)
    {
        _applicableContestants = applicalbleContestants;
        _sidePotAmount = sidePotAMount;
    }

    public int pot
    {
        get { return _sidePotAmount; }
    }

    public List<Contestant> contenders
    {
        get { return _applicableContestants; }
    }
}