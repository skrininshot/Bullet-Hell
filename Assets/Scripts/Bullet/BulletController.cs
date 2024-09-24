using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class BulletController
{
    private readonly TimeShifter _timeShifter;
    private readonly Bullet _bullet;
    private readonly MouseAiming _mouseAiming;
    private readonly BulletAnimationController _animationController;
    
    private readonly Settings _settings;
    private readonly PlayerStateMachine _playerStateMachine;

    private Sequence _lifeCycleSequence;

    private bool _isBulletMiss = false;

    public BulletController(
        TimeShifter timeShifter, 
        Bullet bullet,
        MouseAiming mouseAiming,
        BulletAnimationController animationController,
        GameSettings settings,
        PlayerStateMachine playerStateMachine)
    {
        _timeShifter = timeShifter;
        _bullet = bullet;
        _bullet.gameObject.SetActive(false);
        _mouseAiming = mouseAiming;
        _animationController = animationController;
        
        _settings = settings.Bullet.Controller;
        _playerStateMachine = playerStateMachine;
    }

    public void Initialize()
    {
        _timeShifter.RegisterUser(this, _settings.TimeShiftValue);

        Spawn();

        CreateLifeCycleSquence();

        _animationController.Initialize();
        _animationController.OnAnimationComplete += OnAnimationComplete;

        _bullet.OnHit += OnBulletHit;
    }

    public void Dispose()
    {
        _timeShifter.UnregisterUser(this);

        Despawn();

        _lifeCycleSequence.Kill();
        _lifeCycleSequence = null;

        _animationController.OnAnimationComplete -= OnAnimationComplete;
        _animationController.Dispose();

        _bullet.OnHit -= OnBulletHit;

        _isBulletMiss = false;
    }

    private void CreateLifeCycleSquence()
    {
        _lifeCycleSequence = DOTween.Sequence();

        _lifeCycleSequence.PrependInterval(_settings.BulletLifeTime);

        _lifeCycleSequence.OnComplete(() => { ReturnToAimingState(); });

        _lifeCycleSequence.SetUpdate(false);
        _lifeCycleSequence.Play();
    }

    private void Spawn()
    {
        _bullet.transform.position = _mouseAiming.BulletSpawnPoint.position;
        _bullet.transform.eulerAngles = _mouseAiming.BulletSpawnPoint.eulerAngles;
        _bullet.gameObject.SetActive(true);
    }

    private void Despawn()
    {
        if (_bullet.gameObject != null)
            _bullet.gameObject.SetActive(false);
    }

    private void OnBulletHit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage();

            if (!_animationController.AnimationIsPlaying)
                _animationController.PlayNewAnimation();
        }
        else
        {
            _isBulletMiss = true;

            if (!_animationController.AnimationIsPlaying) 
                ReturnToAimingState();
        }       
    }

    private void OnAnimationComplete()
    {
        if (_isBulletMiss)  
            ReturnToAimingState();
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