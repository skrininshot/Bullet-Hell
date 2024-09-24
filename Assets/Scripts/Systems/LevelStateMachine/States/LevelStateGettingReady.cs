using Zenject;

public class LevelStateGettingReady : State
{
    private readonly LevelStateMachine _levelStateMachine;

    public LevelStateGettingReady (LevelStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }

    public override void Start()
    {
        _levelStateMachine.ChangeState((int)LevelStates.Playing);
    }

    public class Factory : PlaceholderFactory<LevelStateGettingReady> { }
}
