using System;
using Zenject;

public class PlayerStateAiming : State
{
    private Settings _settings;
    private PlayerStateMachine _playerStateMachine;
    private MouseAiming _mouseAiming;
    private CameraMover _cameraMover;

    public PlayerStateAiming (GameSettings settings, PlayerStateMachine playerStateMachine, 
        CameraMover cameraMover, 
        MouseAiming mouseAiming)
    {
        _settings = settings.Player.PlayerStates.AimingState;
        _playerStateMachine = playerStateMachine;
        _cameraMover = cameraMover;
        _mouseAiming = mouseAiming;
    }

    public override void Start()
    {
        _mouseAiming.enabled = true;
        _mouseAiming.OnClick.AddListener(MouseClick);
        _cameraMover.SetTransform(_mouseAiming.transform, _settings.CameraMoveToAimingSpeed);
    }

    public override void Dispose()
    {
        _mouseAiming.enabled = false;
        _mouseAiming.OnClick.RemoveListener(MouseClick);
    }

    private void MouseClick()
    {
        _playerStateMachine.ChageState((int)PlayerStates.Bullet);
    }

    [Serializable]
    public class Settings
    {
        public float CameraMoveToAimingSpeed = 0.1f;
    }

    public class Factory : PlaceholderFactory<PlayerStateAiming> { }
}
