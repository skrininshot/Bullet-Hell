public class PlayerStateMachine : StateMachine
{
    public PlayerStateMachine(PlayerStateFactory factory)
    {
        _factory = factory;
    }

    public override void Initialize()
    {
        ChangeState((int)PlayerStates.Aiming);
    }
}
