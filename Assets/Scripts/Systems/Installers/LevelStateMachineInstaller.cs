using Zenject;

public class LevelStateMachineInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LevelStateFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelStateMachine>().AsSingle();
        Container.BindFactory<LevelStateGettingReady, LevelStateGettingReady.Factory>().WhenInjectedInto<LevelStateFactory>();
        Container.BindFactory<LevelStatePlaying, LevelStatePlaying.Factory>().WhenInjectedInto<LevelStateFactory>();
        Container.BindFactory<LevelStateAward, LevelStateAward.Factory>().WhenInjectedInto<LevelStateFactory>();
    }
}