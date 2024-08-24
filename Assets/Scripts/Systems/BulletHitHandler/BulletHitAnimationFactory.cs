using ModestTree;

public enum BulletHitAnimations
{
    OnlyTimeShift,
    Orbit
}

public class BulletHitAnimationFactory
{
    readonly BulletHitAnimationOnlyTimeShift.Factory _onlyTimeShiftFactory;
    readonly BulletHitAnimationOrbit.Factory _orbitFactory;

    public BulletHitAnimationFactory(
        BulletHitAnimationOnlyTimeShift.Factory onlyTimeShiftFactory,
        BulletHitAnimationOrbit.Factory orbitFactory)
    {
        _onlyTimeShiftFactory = onlyTimeShiftFactory;
        _orbitFactory = orbitFactory;
    }

    public BulletHitAnimation CreateState(int state)
    {
        switch (state)
        {
            case (int)BulletHitAnimations.OnlyTimeShift:
                {
                    return _onlyTimeShiftFactory.Create();
                }
            case (int)BulletHitAnimations.Orbit:
                {
                    return _orbitFactory.Create();
                }
            default:
                {
                    throw Assert.CreateException();
                }
        }
    }
}
