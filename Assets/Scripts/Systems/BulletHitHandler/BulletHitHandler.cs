using System.Collections.Generic;
using UnityEngine;

public class BulletHitHandler
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly BulletAnimationController _animationController;
    
    private readonly Bullet _bullet;
    private HashSet<IDamagable> _hitedObjects = new();
    
    private bool _exitStateAfterAnimation = false;

    public BulletHitHandler(PlayerStateMachine playerStateMachine,
        BulletAnimationController animator, Bullet bullet)
    {
        _playerStateMachine = playerStateMachine;
        _animationController = animator;
        _bullet = bullet;
    }

    public void Initialize()
    {
        _animationController.OnAnimationComplete += OnAnimationComplete;

        _bullet.OnHit.AddListener(PlayBulletHitAnimation);
    }

    public void Dispose()
    {
        _animationController.OnAnimationComplete -= OnAnimationComplete;

        _bullet.OnHit.RemoveListener(PlayBulletHitAnimation);
    }

    public void PlayBulletHitAnimation(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IDamagable damagable))
        {
            _hitedObjects.Add(damagable);
            _animationController.PlayNewAnimation();
        }
        else
        {
            if (_animationController.AnimationIsPlaying)
                _exitStateAfterAnimation = true;
            else
                RetunToAimingState();
        }
    }

    private void OnAnimationComplete()
    {
        Damage();

        if (_exitStateAfterAnimation)
        {
            _exitStateAfterAnimation = false;
            RetunToAimingState();
        }
    }

    private void RetunToAimingState() =>
        _playerStateMachine.ChangeState((int)PlayerStates.Aiming);

    private void Damage()
    {
        if (_hitedObjects == null) return;

        foreach (var obj in _hitedObjects)
            obj.Damage();

        _hitedObjects.Clear();
    }
}
