public abstract class UIViewController<TView>
    where TView : PauseBaseView
{
    protected readonly SceneTransition _sceneTransition;
    protected readonly PauseSystem _pauseSystem;
    protected readonly TView _pauseView;

    protected UIViewController(SceneTransition sceneTransition, PauseSystem pauseSystem, TView pauseView)
    {
        _sceneTransition = sceneTransition;
        _pauseSystem = pauseSystem;
        _pauseView = pauseView;
    }

    public virtual void Initialize()
    {
        _pauseView.RestartButton.onClick.AddListener(_sceneTransition.RestartScene);
        _pauseView.MenuButton.onClick.AddListener(_sceneTransition.TransitionToMenu);
    }

    public virtual void Dispose()
    {
        _pauseView.RestartButton.onClick.RemoveListener(_sceneTransition.RestartScene);
        _pauseView.MenuButton.onClick.RemoveListener(_sceneTransition.TransitionToMenu);
    }

    public virtual void Show(bool value)
    {
        _pauseSystem.SetPause(value);
        _pauseView.SetVisibility(value);
    }
}
