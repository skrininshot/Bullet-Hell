using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class BulletController
{
    private readonly TimeShifter _timeShifter;
    private readonly Bullet _bullet;
    private readonly Transform _bulletSpawnPoint;
    private readonly BulletHitHandler _bulletHitHandler;
    private readonly BulletAnimationController _animationController;
    private readonly AirFlowEffectSpawner _airFlowSpawner;

    private readonly Settings _settings;
    private readonly PlayerStateMachine _playerStateMachine;

    private Sequence _lifeCycleSequence;

    public BulletController(
        TimeShifter timeShifter, 
        Bullet bullet,
        [Inject(Id = "BulletSpawnPoint")] Transform bulletSpawnPoint, 
        BulletHitHandler bulletHitHandler,
        BulletAnimationController animationController, 
        AirFlowEffectSpawner airFlowSpawner,
        GameSettings settings,
        PlayerStateMachine playerStateMachine)
    {
        _timeShifter = timeShifter;
        _bullet = bullet;
        _bullet.gameObject.SetActive(false);
        _bulletSpawnPoint = bulletSpawnPoint;
        _bulletHitHandler = bulletHitHandler;
        _animationController = animationController;
        _airFlowSpawner = airFlowSpawner;
        _settings = settings.Bullet.Controller;
        _playerStateMachine = playerStateMachine;
    }

    public void Initialize()
    {
        _timeShifter.RegisterUser(this, _settings.TimeShiftValue);

        _bullet.transform.position = _bulletSpawnPoint.position;
        _bullet.transform.eulerAngles = _bulletSpawnPoint.eulerAngles;
        _bullet.gameObject.SetActive(true);

        _bulletHitHandler.Initialize();
        _animationController.Initialize();

        _airFlowSpawner.Start();

        CreateSquence();
    }

    public void Dispose()
    {
        _bulletHitHandler.Dispose();
        _animationController.Dispose();

        if (_bullet.gameObject.activeSelf)
            _bullet.gameObject.SetActive(false);

        _airFlowSpawner.Stop();

        _timeShifter.UnregisterUser(this);
    }

    private void CreateSquence()
    {
        _lifeCycleSequence = DOTween.Sequence();
        _lifeCycleSequence.PrependInterval(_settings.BulletLifeTime);

        _lifeCycleSequence.OnComplete(() => { ReturnToAimingState(); });

        _lifeCycleSequence.SetUpdate(false);
        _lifeCycleSequence.Play();
    }

    private void ReturnToAimingState() =>
        _playerStateMachine.ChangeState((int)PlayerStates.Aiming);

    [Serializable]
    public class Settings
    {
        public float CameraMoveToBulletSpeed = 0.5f;
        public float TimeShiftValue = 0.05f;
        public float BulletLifeTime = 0.5f;
    }
}