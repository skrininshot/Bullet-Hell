using UnityEngine;

public class BulletHitHandler
{
    private PlayerStateMachine _playerStateMachine;
    private BulletHitAnimationFactory _animationFactory;
    private BulletHitAnimation _currentAnimation;

    public BulletHitHandler(PlayerStateMachine playerStateMachine, 
        BulletHitAnimationFactory animationFactory)
    {
        _playerStateMachine = playerStateMachine;
        _animationFactory = animationFactory;
    }

    public void HandleHit(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage();
            PlayAnimation();
        }
        else
        {
            ReturnAimingState();
        }
    }

    private void ReturnAimingState() => _playerStateMachine.ChageState((int)PlayerStates.Aiming);

    private void PlayAnimation()
    {
        _currentAnimation?.Dispose();

        _currentAnimation = 
            _animationFactory.CreateState((int)BulletHitAnimations.Orbit);

        _currentAnimation.Play();
    }
}
