public class AwardViewController : UIViewController<AwardView>
{
    public AwardViewController(SceneTransition sceneTransition, PauseSystem pauseSystem, AwardView awardView)
        : base(sceneTransition, pauseSystem, awardView)
    { }

    public override void Initialize()
    {
        base.Initialize();
        Show(true);
        _view.NextLevelButton.onClick.AddListener(HandleNextLevelButton);
    }

    private void HandleNextLevelButton() => _sceneTransition.TransitionToNextLevel();

    public override void Dispose()
    {
        Show(false);
        _view.NextLevelButton.onClick.RemoveListener(HandleNextLevelButton);
        base.Dispose();
    }
}