public abstract class UIViewController<TView>
    where TView : PauseBaseView
{
    protected readonly SceneTransition _sceneTransition;
    protected readonly TView _view;

    protected UIViewController(SceneTransition sceneTransition, TView pauseView)
    {
        _sceneTransition = sceneTransition;
        _view = pauseView;
    }

    public virtual void Initialize()
    {
        _view.RestartButton.onClick.AddListener(_sceneTransition.RestartScene);
        _view.MenuButton.onClick.AddListener(_sceneTransition.TransitionToMenu);
    }

    public virtual void Dispose()
    {
        _view.RestartButton.onClick.RemoveListener(_sceneTransition.RestartScene);
        _view.MenuButton.onClick.RemoveListener(_sceneTransition.TransitionToMenu);
    }

    public virtual void Show(bool value)
    {
        _view.SetVisibility(value);
    }
}
