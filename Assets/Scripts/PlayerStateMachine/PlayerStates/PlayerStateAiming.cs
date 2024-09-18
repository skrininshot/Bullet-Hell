using System;
using Zenject;

public class PlayerStateAiming : State
{
    private Settings _settings;
    private PlayerStateMachine _playerStateMachine;
    private MouseAiming _mouseAiming;
    private AimingView _aimingView;
    private CameraMover _cameraMover;

    public PlayerStateAiming (GameSettings settings, PlayerStateMachine playerStateMachine, 
        CameraMover cameraMover, MouseAiming mouseAiming, AimingView aimingUI)
    {
        _settings = settings.Player.States.AimingState;
        _playerStateMachine = playerStateMachine;
        _cameraMover = cameraMover;
        _mouseAiming = mouseAiming;
        _aimingView = aimingUI;
    }

    public override void Start()
    {
        if (_aimingView) _aimingView.gameObject.SetActive(true);

        if (_mouseAiming) _mouseAiming.gameObject.SetActive(true);

        _mouseAiming.OnClick.AddListener(MouseClick);
        _cameraMover.SetTransform(_mouseAiming.transform, _settings.CameraMoveToAimingSpeed);
    }

    public override void Dispose()
    {
        if (_aimingView) _aimingView.gameObject.SetActive(false);

        if (_mouseAiming) _mouseAiming.gameObject.SetActive(false);

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
