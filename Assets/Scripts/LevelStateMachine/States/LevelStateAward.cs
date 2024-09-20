using Zenject;

public class LevelStateAward : State
{
    private readonly LevelStateMachine _levelStateMachine;
    private readonly AwardViewController _awardViewController;

    public LevelStateAward(LevelStateMachine levelStateMachine, AwardViewController awardViewController)
    {
        _levelStateMachine = levelStateMachine;
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
