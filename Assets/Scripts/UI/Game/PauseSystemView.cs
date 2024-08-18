using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseSystemView : MonoBehaviour
{
    
    public Button ContinueButton => _continueButton;
    public Button MenuButton => _menuButton;

    private Settings _settings;

    [SerializeField] private Transform _popup;
    [SerializeField] private float _showX = 0;
    [SerializeField] private float _hideX = -1000;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _menuButton;

    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _settings = gameSettings.UI.Game.PauseSystem;
    }

    private void Start()
    {
        _popup.transform.localPosition = new Vector3(_hideX, 0, 0);

        Pause(false);
    }

    public void Pause(bool value)
    {
        DOTween.Kill(_popup.transform, true);

        float endValue = value ? _showX : _hideX;
        _popup.transform.DOLocalMoveX(endValue, _settings.ShowHideSpeed).SetUpdate(true);
    }

    [Serializable]
    public class Settings
    {
        public float ShowHideSpeed = 0.2f;
    }
}