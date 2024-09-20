using Zenject;

public class LevelStatePlaying : State
{
    private readonly LevelStateMachine _levelStateMachine;
    private readonly PauseViewController _pauseViewController;

    public LevelStatePlaying(LevelStateMachine levelStateMachine, PauseViewController pauseViewController)
    {
        _levelStateMachine = levelStateMachine;
        _pauseViewController = pauseViewController;
    }

    public override void Start()
    {
        _pauseViewController.Initialize();
    }

    //CHECK FOR LEVEL COMPLETE AND START NEXT STATE

    public override void Dispose()
    {
        _pauseViewController.Dispose();
    }

    public class Factory : PlaceholderFactory<LevelStatePlaying> { }
}
