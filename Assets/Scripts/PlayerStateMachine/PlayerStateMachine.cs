public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(PlayerStateFactory factory)
    {
        _factory = factory;
    }
}
