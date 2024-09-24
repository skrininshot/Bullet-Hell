using Zenject;

public class LevelStatePlaying : State
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly LevelStateMachine _levelStateMachine;
    private readonly PauseViewController _pauseViewController;
    private readonly ObjectiveTracker _objectiveTracker;

    public LevelStatePlaying(PlayerStateMachine playerStateMachine,
        LevelStateMachine levelStateMachine, 
        PauseViewController pauseViewController,
        ObjectiveTracker objectiveTracker)
    {
        _playerStateMachine = playerStateMachine;
        _levelStateMachine = levelStateMachine;
        _pauseViewController = pauseViewController;
        _objectiveTracker = objectiveTracker;
    }

    public override void Start()
    {
        _pauseViewController.Initialize();
        _playerStateMachine.ChangeState((int)PlayerStates.Aiming);
        _objectiveTracker.OnObjectivesComplete += OnObjectivesComplete;
    }

    private void OnObjectivesComplete()
    {
        _levelStateMachine.ChangeState((int)LevelStates.Award);
    }

    public override void Dispose()
    {
        _pauseViewController.Dispose();
        _objectiveTracker.OnObjectivesComplete -= OnObjectivesComplete;
    }

    public class Factory : PlaceholderFactory<LevelStatePlaying> { }
}
