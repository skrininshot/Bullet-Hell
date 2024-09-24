using ModestTree;

public enum PlayerStates
{
    Default,
    Aiming,
    Bullet
}

public class PlayerStateFactory : StateFactory
{
    readonly PlayerStateAiming.Factory _aimingFactory;
    readonly PlayerStateBullet.Factory _bulletFactory;
    readonly PlayerStateDefault.Factory _defaultFactory;

    public PlayerStateFactory(
        PlayerStateAiming.Factory aimingFactory,
        PlayerStateBullet.Factory bulletFactory,
        PlayerStateDefault.Factory defaultFactory)
    {
        _aimingFactory = aimingFactory;
        _bulletFactory = bulletFactory;
        _defaultFactory = defaultFactory;
    }

    public override State CreateState(int state)
    {
        switch (state)
        {
            case (int)PlayerStates.Default:
                return _defaultFactory.Create();

            case (int)PlayerStates.Aiming:
                    return _aimingFactory.Create();

            case (int)PlayerStates.Bullet:
                    return _bulletFactory.Create();

            default:
                    throw Assert.CreateException();
        }
    }
}
