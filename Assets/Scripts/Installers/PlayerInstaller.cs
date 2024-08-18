using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MouseAiming _mouseAiming;
    [SerializeField] private Bullet _bulletInstance;
    [SerializeField] private Transform _bulletSpawnPoint;

    public override void InstallBindings()
    {
        InstallCamera();
        InstallMouseAiming();
        InstallBullet();
        InstallPlayerStateMachine();
    }

    private void InstallCamera()
    {
        Container.BindInterfacesAndSelfTo<CameraMover>().AsSingle().WithArguments(_camera);
    }
    private void InstallMouseAiming()
    {
        Container.BindInstance(_mouseAiming).AsSingle();
    }

    private void InstallBullet()
    {
        Container.BindInstance(_bulletSpawnPoint).WithId("BulletSpawnPoint").AsSingle();

        Container.BindFactory<Bullet, Bullet.Factory>()
                .FromComponentInNewPrefab(_bulletInstance)
                .WithGameObjectName("Bullet");
    }

    private void InstallPlayerStateMachine()
    {
        Container.Bind<PlayerStateFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle();
        Container.BindFactory<PlayerStateAiming, PlayerStateAiming.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateBullet, PlayerStateBullet.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateHit, PlayerStateHit.Factory>().WhenInjectedInto<PlayerStateFactory>();
    }
}
