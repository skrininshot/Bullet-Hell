public class GameStateMachine : StateMachine
{
    public GameStateMachine(GameStateFactory factory)
    {
        _factory = factory;
    }
}