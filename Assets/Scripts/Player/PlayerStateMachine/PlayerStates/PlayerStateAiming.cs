using System;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerStateAiming : State
{
    private readonly Settings _settings;
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly MouseAiming _mouseAiming;
    private readonly AimingView _aimingView;
    private readonly CameraMover _cameraMover;

    private readonly PlayerInput _playerInput;

    public PlayerStateAiming (GameSettings settings, PlayerStateMachine playerStateMachine, 
        PlayerInput playerInput, CameraMover cameraMover, 
        MouseAiming mouseAiming, AimingView aimingUI)
    {
        _settings = settings.Player.States.AimingState;
        _playerStateMachine = playerStateMachine;
        _playerInput = playerInput;
        _cameraMover = cameraMover;
        _mouseAiming = mouseAiming;
        _aimingView = aimingUI;
    }

    public override void Start()
    {
        _aimingView.gameObject.SetActive(true);
        _mouseAiming.enabled = true;

        _cameraMover.SetTransform(_mouseAiming.transform, _settings.CameraMoveToAimingSpeed);

        _playerInput.PC.Shoot.started += ChangeStateToBullet;
    }

    public override void Dispose()
    {
        _aimingView.gameObject.SetActive(false);
        _mouseAiming.enabled = false;

        _playerInput.PC.Shoot.started -= ChangeStateToBullet;
    }

    private void ChangeStateToBullet(InputAction.CallbackContext context)
    {
        _playerStateMachine.ChangeState((int)PlayerStates.Bullet);
    }

    [Serializable]
    public class Settings
    {
        public float CameraMoveToAimingSpeed = 0.2f;
    }

    public class Factory : PlaceholderFactory<PlayerStateAiming> { }
}
