using Zenject;

public class TimeShifterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TimeShifter>().AsSingle();
    }
}
