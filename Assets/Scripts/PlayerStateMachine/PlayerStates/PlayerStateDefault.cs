using System;
using UnityEngine;
using Zenject;

public class PlayerStateDefault : State
{
    private readonly Settings _settings;
    private readonly Transform _cameraDefaultPoint;
    private readonly CameraMover _cameraMover;

    public PlayerStateDefault( GameSettings gameSettings,
        [Inject(Id = "CameraDefaultPoint")] Transform cameraDefaultPoint, 
        CameraMover cameraMover)
    {
        _settings = gameSettings.Player.States.DefaultState;
        _cameraDefaultPoint = cameraDefaultPoint;
        _cameraMover = cameraMover;
    }

    public override void Start()
    {
        _cameraMover.SetTransform(_cameraDefaultPoint, _settings.CameraMoveToDefaultPointSpeed);
    }

    [Serializable]
    public class Settings
    {
        public float CameraMoveToDefaultPointSpeed = 0.1f;
    }


    public class Factory : PlaceholderFactory<PlayerStateDefault> { }

}