using UnityEngine;
using Zenject;

public class GameUIInstaller : MonoInstaller
{
    [SerializeField] private AimingView _aimingView;
    [SerializeField] private PauseView _pauseView;
    [SerializeField] private AwardView _awardView;

    public override void InstallBindings()
    {
        InstallSystems();
        InstallViews();
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
}
