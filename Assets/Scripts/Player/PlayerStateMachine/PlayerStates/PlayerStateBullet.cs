using Zenject;

public class PlayerStateBullet : State
{
    private BulletController _bulletController;

    public PlayerStateBullet(BulletController bulletController)
    {
        _bulletController = bulletController;
    }

    public override void Start()
    {
        _bulletController.Initialize();
    }

    public override void Dispose()
    {
        _bulletController.Dispose();
    }

    public class Factory : PlaceholderFactory<PlayerStateBullet> { }
}
