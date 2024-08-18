public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(PlayerStateFactory factory)
    {
        _factory = factory;
    }

    public override void Initialize()
    {
        ChageState((int)PlayerStates.Aiming);
    }
}
