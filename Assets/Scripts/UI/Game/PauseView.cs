using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseView : MonoBehaviour
{
    public Button ContinueButton => _continueButton;
    public Button MenuButton => _menuButton;

    private Settings _settings;

    private Tween _showAnimation;

    [SerializeField] private Transform _popup;
    [SerializeField] private float _showX = 0;
    [SerializeField] private float _hideX = -1000;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _menuButton;

    private CursorController _cursorController;

    [Inject]
    private void Construct(GameSettings gameSettings, CursorController cursorController)
    {
        _settings = gameSettings.UI.Game.PauseView;
        _cursorController = cursorController;
    }

    private void Start()
    {
        _popup.transform.localPosition = new Vector3(_hideX, 0, 0);

        Pause(false);
    }

    public void Pause(bool value)
    {
        if (_showAnimation.IsActive())
            _showAnimation.Kill(true);

        if (value)
        {
            _showAnimation = _popup.transform
                .DOLocalMoveX(_showX, _settings.AppearingDuration)
                .SetUpdate(true);

            _cursorController.RegisterUser(this);
        }
        else
        {
            _showAnimation = _popup.transform
                .DOLocalMoveX(_hideX, _settings.DisappearingDuration)
                .SetUpdate(true);

            _cursorController.UnregisterUser(this);
        }
    }

    [Serializable]
    public class Settings
    {
        public float AppearingDuration = 0.2f;
        public float DisappearingDuration = 0.1f;
    }
}