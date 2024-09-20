using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class PlayerStateBullet : State, IPausable
{
    private Settings _settings;
    private PlayerStateMachine _playerStateMachine;
    private Bullet _bullet;
    private Transform _bulletSpawnPoint;
    private CameraMover _cameraMover;
    private TimeShifter _timeShifter;
    private PauseSystem _pauseSystem;
    private BulletHitHandler _bulletHitHandler;
    private AirFlowEffectSpawner _airFlowSpawner;

    private Sequence _sequence;

    public PlayerStateBullet(GameSettings settings, PlayerStateMachine playerStateMachine, Bullet bullet, 
        [Inject(Id = "BulletSpawnPoint")] Transform bulletSpawnPoint, CameraMover cameraMover, TimeShifter timeShifter, 
        PauseSystem pauseSystem, BulletHitHandler bulletHitHandler, AirFlowEffectSpawner airFlowSpawner)
    {
        _settings = settings.Player.States.BulletState;
        _playerStateMachine = playerStateMachine;
        _bullet = bullet;
        _bulletSpawnPoint = bulletSpawnPoint;
        _cameraMover = cameraMover;
        _timeShifter = timeShifter;
        _pauseSystem = pauseSystem;
        _bulletHitHandler = bulletHitHandler;
        _airFlowSpawner = airFlowSpawner;

        _bullet.gameObject.SetActive(false);
    }

    public override void Start()
    {
        _timeShifter.RegisterUser(this, _settings.TimeShiftValue);

        _bullet.transform.position = _bulletSpawnPoint.position;
        _bullet.transform.eulerAngles = _bulletSpawnPoint.eulerAngles; 
        _bullet.gameObject.SetActive(true);

        _bulletHitHandler.Initialize();

        _airFlowSpawner.Start();

        _cameraMover.SetTransform(_bullet.CameraPoint, _settings.CameraMoveToBulletSpeed);

        _pauseSystem.RegisterPausable(this);

        CreateSquence();
    }

    private void CreateSquence()
    {
        _sequence = DOTween.Sequence();
        _sequence.PrependInterval(_settings.BulletLifeTime);

        _sequence.OnComplete(() => { ReturnToAimingState();});

        _sequence.SetUpdate(false);
        _sequence.Play();
    }

    private void ReturnToAimingState() => 
        _playerStateMachine.ChangeState((int)PlayerStates.Aiming);

    public override void Dispose()
    {
        _sequence.Kill();

        _bulletHitHandler.Dispose();

        if (_bullet.gameObject.activeSelf)
            _bullet.gameObject.SetActive(false);

        _airFlowSpawner.Stop();

        _timeShifter.UnregisterUser(this);
        _pauseSystem.UnregisterPausable(this);
    }

    public void Pause()
    {
        if (_sequence.IsActive())
            _sequence.Pause();
    }

    public void Resume()
    {
        if (_sequence.IsActive())
            _sequence.Play();
    }

    [Serializable]
    public class Settings
    {
        public float CameraMoveToBulletSpeed = 0.1f;
        public float TimeShiftValue = 0.25f;
        public float BulletLifeTime = 10f;
    }

    public class Factory : PlaceholderFactory<PlayerStateBullet> { }
}
