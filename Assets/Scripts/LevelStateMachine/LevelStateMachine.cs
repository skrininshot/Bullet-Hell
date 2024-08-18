public class LevelStateMachine : StateMachine
{
    public LevelStateMachine(LevelStateFactory factory)
    {
        _factory = factory;
    }
}