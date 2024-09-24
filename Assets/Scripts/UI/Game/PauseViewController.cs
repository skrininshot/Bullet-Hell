using UnityEngine.InputSystem;

public class PauseViewController : UIViewController<PauseView>
{
    private PlayerInput _playerInput;

    public PauseViewController(SceneTransition sceneTransition, PlayerInput playerInput, PauseSystem pauseSystem, PauseView pauseView)
        : base(sceneTransition, pauseSystem, pauseView)
    {
        _playerInput = playerInput;
    }

    public override void Initialize()
    {
        _playerInput.PC.Pause.performed += RevertPause;
        _view.ContinueButton.onClick.AddListener(HandleContinueButton);
        base.Initialize();
    }

    public override void Dispose()
    {
        _playerInput.PC.Pause.performed -= RevertPause;
        _view.ContinueButton.onClick.RemoveListener(HandleContinueButton);
        base.Dispose();
    }

    private void HandleContinueButton() => Show(false);

    public void RevertPause(InputAction.CallbackContext context)
    {
        Show(!_pauseSystem.IsPaused);
    }
}
