using System;
using UnityEngine;

public class BulletHitHandler
{
    private PlayerStateMachine _playerStateMachine;
    private BulletHitAnimationFactory _animationFactory;
    private BulletHitAnimation _currentAnimation;

    bool _returnToAimingStateAfterAnimation = false;

    public BulletHitHandler(PlayerStateMachine playerStateMachine, 
        BulletHitAnimationFactory animationFactory)
    {
        _playerStateMachine = playerStateMachine;
        _animationFactory = animationFactory;
    }
    public void Initialize() => CreateNewAnimation();

    public void Dispose() => DisposeCurrentAnimation();

    public void HandleHit(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage();
            PlayNewAnimation();
        }
        else
        {
            if (_currentAnimation.IsActive)
                _returnToAimingStateAfterAnimation = true;
            else
                ReturnToAimingState();  
        }
    }

    private void ReturnToAimingState() => _playerStateMachine.ChageState((int)PlayerStates.Aiming);

    private void PlayNewAnimation()
    {
        if (_currentAnimation.IsActive) return;

        DisposeCurrentAnimation();

        CreateNewAnimation();

        _currentAnimation.Play();
    }

    private void CreateNewAnimation()
    {
        _currentAnimation =
           _animationFactory.CreateAnimation((int)BulletHitAnimations.Orbit);

        _currentAnimation.Create();
        _currentAnimation.OnComplete += OnAnimationComplete; 
    }

    private void DisposeCurrentAnimation()
    {
        if (_currentAnimation == null) return;
        
        _currentAnimation?.Dispose();
        _currentAnimation.OnComplete -= OnAnimationComplete;
    }

    private void OnAnimationComplete()
    {
        if (!_returnToAimingStateAfterAnimation) return;
        
        _returnToAimingStateAfterAnimation = false;
        ReturnToAimingState();
    }  
}
