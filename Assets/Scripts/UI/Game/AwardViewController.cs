using UnityEngine.InputSystem;

public class AwardViewController : UIViewController<AwardView>
{
    private readonly PlayerInput _playerInput;

    public AwardViewController(SceneTransition sceneTransition, AwardView awardView, PlayerInput playerInput)
        : base(sceneTransition, awardView)
    {
        _playerInput = playerInput;
    }

    private bool _isVisible = false;

    public override void Initialize()
    {
        base.Initialize();

        Show(true);

        if (!_sceneTransition.CanTransitionToNextLevel())
            _view.NextLevelButton.interactable = false;

        _view.NextLevelButton.onClick.AddListener(HandleNextLevelButton);
        _playerInput.PC.Pause.performed += ChangeVisibility;
    }

    private void HandleNextLevelButton() => _sceneTransition.TransitionToNextLevel();

    public override void Dispose()
    {
        _playerInput.PC.Pause.performed -= ChangeVisibility;
        _view.NextLevelButton.onClick.RemoveListener(HandleNextLevelButton);

        base.Dispose();
    }

    public override void Show(bool value)
    {
        base.Show(value);
        _isVisible = value;
    }

    private void ChangeVisibility(InputAction.CallbackContext context) => Show(!_isVisible);
}