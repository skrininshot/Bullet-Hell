using Zenject;

public class LevelStatePlaying : State
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly LevelStateMachine _levelStateMachine;
    private readonly PauseViewController _pauseViewController;
    private readonly ObjectiveTracker _objectiveTracker;
    
    private bool _objectivesIsComplete = false;

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
        _playerStateMachine.OnStateChange += OnPlayerStateMachineStateChanged;
    }

    private void OnObjectivesComplete()
    {
        _objectivesIsComplete = true;
        CheckConditionsToComplete();
    }

    private void OnPlayerStateMachineStateChanged(State newState)
    {
        if (newState is PlayerStateAiming)
            CheckConditionsToComplete();
    }

    private void CheckConditionsToComplete()
    {
        if (_objectivesIsComplete && _playerStateMachine.CurrentState is PlayerStateAiming)
            _levelStateMachine.ChangeState((int)LevelStates.Award);
    }

    public override void Dispose()
    {
        _pauseViewController.Dispose();
        _objectiveTracker.OnObjectivesComplete -= OnObjectivesComplete;
        _playerStateMachine.OnStateChange -= OnPlayerStateMachineStateChanged;
    }

    public class Factory : PlaceholderFactory<LevelStatePlaying> { }
}
