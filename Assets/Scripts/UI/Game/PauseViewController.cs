using UnityEngine;
using Zenject;

public class PauseViewController : UIViewController<PauseView>, ITickable
{
    public PauseViewController(SceneTransition sceneTransition, PauseSystem pauseSystem, PauseView pauseView)
        : base(sceneTransition, pauseSystem, pauseView)
    { }

    public override void Initialize()
    {
        _pauseView.ContinueButton.onClick.AddListener(HandleContinueButton);
        base.Initialize();
    }

    public override void Dispose()
    {
        _pauseView.ContinueButton.onClick.RemoveListener(HandleContinueButton);
        base.Dispose();
    }

    private void HandleContinueButton() => Show(false);

    //GET RID OF THIS USING NEW INPUT SYSTEM
    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Show(!_pauseSystem.IsPaused);
    }
}
