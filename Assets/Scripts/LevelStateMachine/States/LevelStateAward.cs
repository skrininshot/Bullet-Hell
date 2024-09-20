using Zenject;

public class LevelStateAward : State
{
    private readonly AwardViewController _awardViewController;

    public LevelStateAward(AwardViewController awardViewController)
    {
        _awardViewController = awardViewController;
    }

    public override void Start()
    {
        _awardViewController.Initialize();
    }

    public override void Dispose()
    {
        _awardViewController.Dispose();
    }

    public class Factory : PlaceholderFactory<LevelStateAward> { }
}
