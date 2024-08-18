using Zenject;

public class CursorControllerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CursorController>().AsSingle();
    }
}