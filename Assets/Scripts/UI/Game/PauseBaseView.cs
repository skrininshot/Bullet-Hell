using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseBaseView : MonoBehaviour
{
    public Button RestartButton => _restartButton;
    public Button MenuButton => _menuButton;

    private BaseSettings _baseSettings;

    private Tween _changeVisibilityAnimation;

    [Header("Popup Settings")]
    [SerializeField] private Transform _popup;
    [SerializeField] private float _showX = 0;
    [SerializeField] private float _hideX = -1000;

    [Header("Buttons")]
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _restartButton;

    private CursorController _cursorController;

    [Inject]
    private void BaseConstruct(GameSettings gameSettings, CursorController cursorController)
    {
        _baseSettings = gameSettings.UI.Game.PauseBaseView;
        _cursorController = cursorController;
    }

    protected virtual void Start()
    {
        _popup.transform.localPosition = new Vector3(_hideX, 0, 0);

        SetVisibility(false);
    }

    public virtual void SetVisibility(bool visible)
    {
        if (_changeVisibilityAnimation.IsActive())
            _changeVisibilityAnimation.Kill(true);

        if (visible)
        {
            _changeVisibilityAnimation = _popup.transform
                .DOLocalMoveX(_showX, _baseSettings.AppearingDuration)
                .SetUpdate(true);

            _cursorController.RegisterUser(this);
        }
        else
        {
            _changeVisibilityAnimation = _popup.transform
                .DOLocalMoveX(_hideX, _baseSettings.DisappearingDuration)
                .SetUpdate(true);

            _cursorController.UnregisterUser(this);
        }
    }

    protected virtual void OnDestroy()
    {
        _cursorController.UnregisterUser(this);
    }

    [Serializable]
    public class BaseSettings
    {
        public float AppearingDuration = 0.2f;
        public float DisappearingDuration = 0.1f;
    }
}