using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    [SerializeField] private Button _startButton;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MainMenuController>().AsSingle().WithArguments(_startButton).NonLazy();
    }
}