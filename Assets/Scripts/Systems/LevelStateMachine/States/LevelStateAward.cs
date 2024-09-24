using Zenject;

public class LevelStateAward : State
{
    private readonly AwardViewController _awardViewController;
    private readonly PlayerStateMachine _playerStateMachine;

    public LevelStateAward(AwardViewController awardViewController, 
        PlayerStateMachine playerStateMachine)
    {
        _awardViewController = awardViewController;
        _playerStateMachine = playerStateMachine;
    }

    public override void Start()
    {
        _playerStateMachine.ChangeState((int)PlayerStates.Default);
        _awardViewController.Initialize();
    }

    public override void Dispose()
    {
        _awardViewController.Dispose();
    }

    public class Factory : PlaceholderFactory<LevelStateAward> { }
}
