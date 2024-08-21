using System;
using UnityEngine;
using Zenject;

public class PlayerStateBullet : State
{
    private Settings _settings;
    private PlayerStateMachine _playerStateMachine;
    private Bullet.Factory _bulletFactory;
    private Bullet _bullet;
    private Transform _bulletSpawnPoint;
    private CameraMover _cameraMover;
    private TimeShifter _timeShifter;

    public PlayerStateBullet(GameSettings settings, PlayerStateMachine playerStateMachine, Bullet.Factory bulletFactory, 
        [Inject(Id = "BulletSpawnPoint")] Transform bulletSpawnPoint, CameraMover cameraMover, TimeShifter timeShifter)
    {
        _settings = settings.Player.PlayerStates.BulletState;
        _playerStateMachine = playerStateMachine;
        _bulletFactory = bulletFactory;
        _bulletSpawnPoint = bulletSpawnPoint;
        _cameraMover = cameraMover;
        _timeShifter = timeShifter;
    }

    public override void Start()
    {
        _timeShifter.TimeShift(_settings.TimeShiftValue);

        _bullet = _bulletFactory.Create();
        _bullet.transform.SetPositionAndRotation(_bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        _bullet.OnDestroy.AddListener(BulletDestroy);

        _cameraMover.SetTransform(_bullet.CameraPoint, _settings.CameraMoveToBulletSpeed);
    }

    private void BulletDestroy(bool collision)
    {
        _playerStateMachine.ChageState((int)PlayerStates.Hit);
    }

    public override void Dispose()
    {
        _bullet.OnDestroy.RemoveListener(BulletDestroy);
        _timeShifter.Reset();
    }

    [Serializable]
    public class Settings
    {
        public float CameraMoveToBulletSpeed = 0.1f;
        public float TimeShiftValue = 0.25f;
    }

    public class Factory : PlaceholderFactory<PlayerStateBullet> { }
}
