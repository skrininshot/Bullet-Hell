using Zenject;

public class TimeShifterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TimeShifter>().AsSingle();
    }
}
