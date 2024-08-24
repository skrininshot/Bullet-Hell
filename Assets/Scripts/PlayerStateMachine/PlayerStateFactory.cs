using ModestTree;

public enum PlayerStates
{
    Aiming,
    Bullet
}

public class PlayerStateFactory : StateFactory
{
    readonly PlayerStateAiming.Factory _aimingFactory;
    readonly PlayerStateBullet.Factory _bulletFactory;

    public PlayerStateFactory(
        PlayerStateAiming.Factory aimingFactory,
        PlayerStateBullet.Factory bulletFactory)
    {
        _aimingFactory = aimingFactory;
        _bulletFactory = bulletFactory;
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

            default:
                {
                    throw Assert.CreateException();
                }
        }
    }
}
