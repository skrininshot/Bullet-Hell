public class LevelStateMachine : StateMachine
{
    public LevelStateMachine(LevelStateFactory factory)
    {
        _factory = factory;
    }

    public override void Initialize()
    {
        ChangeState((int)LevelStates.GettingReady);
    }
}