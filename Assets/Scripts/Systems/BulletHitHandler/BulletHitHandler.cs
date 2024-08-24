public class BulletHitHandler
{
    private BulletHitAnimationFactory _animationFactory;

    private BulletHitAnimation _currentAnimation;

    public BulletHitHandler(BulletHitAnimationFactory animationFactory)
    {
        _animationFactory = animationFactory;
    }

    public void Hit(IDamagable damagable)
    {
        damagable.Damage(); 

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        _currentAnimation?.Dispose();
        _currentAnimation = 
            _animationFactory.CreateState((int)BulletHitAnimations.OnlyTimeShift);
        _currentAnimation.Play();
    }
}
