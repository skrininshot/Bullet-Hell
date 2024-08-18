using Zenject;

public class SceneTransitionInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SceneTransition>().AsSingle();
    }
}
