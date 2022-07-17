public class PoolPokerContestant : Contestant
{
    protected PoolPokerContestantModel Model { get { return (PoolPokerContestantModel)_model; } }
    public virtual void InitializePool(CommunityPool pool)
    {
        Model.SetPool(pool);
    }

    protected override void Awake()
    {
        _model = new PoolPokerContestantModel();
    }
}
