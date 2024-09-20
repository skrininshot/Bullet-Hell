using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AwardView : PauseBaseView
{
    private Settings _settings;

    [SerializeField] private Button _nextLevelButton;

    [Inject]
    private void Construct(GameSettings gameSettings)
    {

    }

    protected override void Start()
    {
        _nextLevelButton.onClick.AddListener(HandleNextLevelButton);

        base.Start();
    }

    private void HandleNextLevelButton() { }

    protected override void OnDestroy()
    {
        _nextLevelButton.onClick.RemoveListener(HandleNextLevelButton);

        base.OnDestroy();
    }

    [Serializable]
    public class Settings
    {
        
    }
}
