public class AwardViewController : UIViewController<AwardView>
{
    public AwardViewController(SceneTransition sceneTransition, PauseSystem pauseSystem, AwardView awardView)
        : base(sceneTransition, pauseSystem, awardView)
    { }

    public override void Initialize()
    {
        base.Initialize();
        Show(true);
    }

    public override void Dispose()
    {
        Show(false);
        base.Dispose();
    }
}