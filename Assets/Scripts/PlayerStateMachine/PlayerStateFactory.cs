using ModestTree;

public enum PlayerStates
{
    Aiming,
    Bullet,
    Hit
}

public class PlayerStateFactory : StateFactory
{
    readonly PlayerStateAiming.Factory _aimingFactory;
    readonly PlayerStateBullet.Factory _bulletFactory;
    readonly PlayerStateHit.Factory _hitFactory;

    public PlayerStateFactory(
        PlayerStateAiming.Factory aimingFactory,
        PlayerStateBullet.Factory bulletFactory,
        PlayerStateHit.Factory hitFactory)
    {
        _aimingFactory = aimingFactory;
        _bulletFactory = bulletFactory;
        _hitFactory = hitFactory;
    }

    public override State CreateState(int state)
    {
        switch (state)
        {
            case (int)PlayerStates.Aiming:
                {
                    return _aimingFactory.Create();
                }
            case (int)PlayerStates.Bullet:
                {
                    return _bulletFactory.Create();
                }
            case (int)PlayerStates.Hit:
                {
                    return _hitFactory.Create();
                }

            default:
                {
                    throw Assert.CreateException();
                }
        }
    }
}
