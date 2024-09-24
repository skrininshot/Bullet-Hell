using Zenject;

public class GameStateMachineInstaller : MonoInstaller
{
    public override void InstallBindings()
    { 
        Container.Bind<GameStateFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        Container.BindFactory<GameStateMenu, GameStateMenu.Factory>().WhenInjectedInto<GameStateFactory>();
        Container.BindFactory<GameStatePlay, GameStatePlay.Factory>().WhenInjectedInto<GameStateFactory>();
        Container.BindFactory<GameStateTransition, GameStateTransition.Factory>().WhenInjectedInto<GameStateFactory>();
    }
}