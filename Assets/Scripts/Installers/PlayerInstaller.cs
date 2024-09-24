using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MouseAiming _mouseAiming;
    [SerializeField] private Transform _cameraDefaultPoint;

    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private AirFlowEffect _airFlowEffectPrefab;

    public override void InstallBindings()
    {
        InstallInput();
        InstallCamera();
        InstallMouseAiming();
        InstallBullet();
        InstallPlayerStateMachine();
    }

    private void InstallInput()
    {
        Container.BindInstance(new PlayerInput()).AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerInputInitializer>().AsSingle();
    }

    private void InstallCamera()
    {
        Container.BindInstance(_camera).WithId("MainCamera").AsSingle();
        Container.BindInterfacesAndSelfTo<CameraZoom>().AsSingle().WithArguments(_camera);
        Container.BindInterfacesAndSelfTo<CameraMover>().AsSingle().WithArguments(_camera);

        Container.Bind<Transform>().WithId("CameraDefaultPoint").FromInstance(_bulletSpawnPoint);
    }

    private void InstallMouseAiming()
    {
        Container.BindInstance(_mouseAiming).AsSingle();
    }

    private void InstallBullet()
    {
        Container.Bind<Transform>().WithId("BulletSpawnPoint").FromInstance(_bulletSpawnPoint);

        Container.Bind<Bullet>().FromComponentInNewPrefab(_bulletPrefab).AsSingle();

        Container.Bind<BulletController>().AsSingle();
        Container.Bind<BulletHitHandler>().AsSingle();
        Container.Bind<BulletAnimationController>().AsSingle();

        Container.Bind<BulletHitAnimationFactory>().AsSingle();
        Container.BindFactory<BulletHitAnimationOnlyTimeShift, BulletHitAnimationOnlyTimeShift.Factory>().WhenInjectedInto<BulletHitAnimationFactory>();
        Container.BindFactory<BulletHitAnimationOrbit, BulletHitAnimationOrbit.Factory>().WhenInjectedInto<BulletHitAnimationFactory>();

        Container.Bind<AirFlowEffectSpawner>().AsSingle();
        Container.BindFactory<Vector3, Vector3, AirFlowEffect.Settings, float, AirFlowEffect, AirFlowEffect.Factory>()
            .FromPoolableMemoryPool<Vector3, Vector3, AirFlowEffect.Settings, float, AirFlowEffect, AirFlowEffectPool>
            (poolBinder => poolBinder
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_airFlowEffectPrefab)
                .UnderTransformGroup("AirFlowEffect"));
    }

    private void InstallPlayerStateMachine()
    {
        Container.Bind<PlayerStateFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateMachine>().AsSingle();
        Container.BindFactory<PlayerStateAiming, PlayerStateAiming.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateBullet, PlayerStateBullet.Factory>().WhenInjectedInto<PlayerStateFactory>();
        Container.BindFactory<PlayerStateDefault, PlayerStateDefault.Factory>().WhenInjectedInto<PlayerStateFactory>();
    }

    public class  AirFlowEffectPool : MonoPoolableMemoryPool<Vector3, Vector3, AirFlowEffect.Settings, float, IMemoryPool, AirFlowEffect> { }
}
