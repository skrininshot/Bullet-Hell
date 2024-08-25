using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class PlayerStateBullet : State, IPausable
{
    private Settings _settings;
    private PlayerStateMachine _playerStateMachine;
    private Bullet.Factory _bulletFactory;
    private Bullet _bullet;
    private Transform _bulletSpawnPoint;
    private CameraMover _cameraMover;
    private TimeShifter _timeShifter;
    private PauseSystem _pauseSystem;

    private Sequence _sequence;

    public PlayerStateBullet(GameSettings settings, PlayerStateMachine playerStateMachine, Bullet.Factory bulletFactory, 
        [Inject(Id = "BulletSpawnPoint")] Transform bulletSpawnPoint, CameraMover cameraMover, TimeShifter timeShifter, 
        PauseSystem pauseSystem)
    {
        _settings = settings.Player.States.BulletState;
        _playerStateMachine = playerStateMachine;
        _bulletFactory = bulletFactory;
        _bulletSpawnPoint = bulletSpawnPoint;
        _cameraMover = cameraMover;
        _timeShifter = timeShifter;
        _pauseSystem = pauseSystem;
    }

    public override void Start()
    {
        _timeShifter.RegisterUser(this, _settings.TimeShiftValue);

        _bullet = _bulletFactory.Create();
        _bullet.transform.position = _bulletSpawnPoint.position;
        _bullet.transform.eulerAngles = _bulletSpawnPoint.eulerAngles; 

        _bullet.OnDestroy.AddListener(BulletDestroy);

        _cameraMover.SetTransform(_bullet.CameraPoint, _settings.CameraMoveToBulletSpeed);

        _pauseSystem.RegisterPausable(this);

        CreateSquence();
    }

    private void CreateSquence()
    {
        _sequence = DOTween.Sequence();
        _sequence.PrependInterval(_settings.BulletLifeTime);
        _sequence.AppendCallback(() => UnityEngine.Object.Destroy(_bullet.gameObject));
        _sequence.AppendCallback(() => ReturnToAimingState());
        _sequence.SetUpdate(true);
        _sequence.Play();
    }

    private void BulletDestroy(bool collision) => ReturnToAimingState();

    private void ReturnToAimingState() => 
        _playerStateMachine.ChageState((int)PlayerStates.Aiming);

    public override void Dispose()
    {
        _sequence.Kill();
        _bullet.OnDestroy.RemoveListener(BulletDestroy);
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
