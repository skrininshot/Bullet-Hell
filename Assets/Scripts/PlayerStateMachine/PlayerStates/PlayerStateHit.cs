using Zenject;

public class PlayerStateHit : State
{
    private PlayerStateMachine _playerStateMachine;

    public PlayerStateHit(PlayerStateMachine playerStateMachine)
    {
        _playerStateMachine = playerStateMachine;
    }

    public override void Start()
    {
        _playerStateMachine.ChageState((int)PlayerStates.Aiming);
    }

    public class Factory : PlaceholderFactory<PlayerStateHit> { }
}
