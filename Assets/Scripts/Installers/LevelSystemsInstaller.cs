using Zenject;

public class LevelSystemsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallTimeShifter();
        InstallPause();
        InstallLevelScoreRecorder();
        InstallObjectiveTracker();
    }

    private void InstallTimeShifter()
    {
        Container.BindInterfacesAndSelfTo<TimeShifter>().AsSingle();
    }

    private void InstallPause()
    {
        Container.BindInterfacesAndSelfTo<PauseSystem>().AsSingle();
    }

    private void InstallLevelScoreRecorder()
    {
        Container.Bind<LevelScoreRecorder>().AsSingle();
    } 
    
    private void InstallObjectiveTracker()
    {
        Container.Bind<ObjectiveTracker>().AsSingle();
    } 
}