using UnityEngine;
using Zenject;

public class GameUIInstaller : MonoInstaller
{
    [SerializeField] private GameObject _aimingUI;
    [SerializeField] private PauseSystemView _pauseSystemView;

    public override void InstallBindings()
    {
        Container.BindInstance(_aimingUI).WithId("AimingUI").AsSingle();
        Container.BindInterfacesAndSelfTo<PauseSystem>().AsSingle().WithArguments(_pauseSystemView);
    }
}
