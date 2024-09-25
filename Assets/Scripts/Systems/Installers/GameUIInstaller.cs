using UnityEngine;
using Zenject;

public class GameUIInstaller : MonoInstaller
{
    [Header("Views")]
    [SerializeField] private AimingView _aimingView;
    [SerializeField] private PauseView _pauseView;
    [SerializeField] private AwardView _awardView;

    [Header("Floating Text")]
    [SerializeField] private FloatingText _floatingTextPrefab;
    [SerializeField] private Canvas _floatingTextCanvasPrefab;

    public override void InstallBindings()
    {
        InstallSystems();
        InstallViews();
        InstallFloatingText();
    }

    private void InstallSystems()
    { 
        Container.BindInterfacesAndSelfTo<PauseViewController>().AsSingle();
        Container.Bind<AwardViewController>().AsSingle();
    }

    private void InstallViews()
    {
        Container.BindInstance(_aimingView).AsSingle();
        Container.BindInstance(_pauseView).AsSingle();
        Container.BindInstance(_awardView).AsSingle();
    }

    private void InstallFloatingText()
    {
        var floatingTextCanvas = Instantiate(_floatingTextCanvasPrefab);

        Container.BindInstance(floatingTextCanvas).AsSingle();
        
        Container.BindInterfacesAndSelfTo<FloatingTextSpawner>().AsSingle();

        Container.BindFactory<Vector3, string, FloatingText.Settings, Transform, FloatingText, FloatingText.Factory>()
            .FromPoolableMemoryPool<Vector3, string, FloatingText.Settings, Transform, FloatingText, FloatingTextPool>
            (poolBinder => poolBinder
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_floatingTextPrefab)
                .UnderTransform(floatingTextCanvas.transform));
    }

    public class FloatingTextPool : MonoPoolableMemoryPool<Vector3, string, FloatingText.Settings, Transform, IMemoryPool, FloatingText> { }
}
