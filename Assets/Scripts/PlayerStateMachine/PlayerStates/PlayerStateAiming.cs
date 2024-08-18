using System;
using UnityEngine;
using Zenject;

public class PlayerStateAiming : State
{
    private Settings _settings;
    private PlayerStateMachine _playerStateMachine;
    private MouseAiming _mouseAiming;
    private GameObject _aimingUI;
    private CameraMover _cameraMover;

    public PlayerStateAiming (GameSettings settings, PlayerStateMachine playerStateMachine, 
        CameraMover cameraMover, MouseAiming mouseAiming, [Inject(Id ="AimingUI")] GameObject aimingUI)
    {
        _settings = settings.Player.PlayerStates.AimingState;
        _playerStateMachine = playerStateMachine;
        _cameraMover = cameraMover;
        _mouseAiming = mouseAiming;
        _aimingUI = aimingUI;
    }

    public override void Start()
    {
        _aimingUI.SetActive(true);
        _mouseAiming.enabled = true;
        _mouseAiming.OnClick.AddListener(MouseClick);
        _cameraMover.SetTransform(_mouseAiming.transform, _settings.CameraMoveToAimingSpeed);
    }

    public override void Dispose()
    {
        _aimingUI.SetActive(false);
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
