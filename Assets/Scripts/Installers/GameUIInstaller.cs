using UnityEngine;
using Zenject;

public class GameUIInstaller : MonoInstaller
{
    [SerializeField] private AimingView _aimingView;
    [SerializeField] private PauseView _pauseView;

    public override void InstallBindings()
    {
        InstallAimingView();
        InstallPause();
    }

    private void InstallAimingView()
    {
        Container.BindInstance(_aimingView).AsSingle();
    }

    private void InstallPause()
    {
        Container.BindInstance(_pauseView).AsSingle();
        Container.BindInterfacesAndSelfTo<PauseSystem>().AsSingle();
    }
}
