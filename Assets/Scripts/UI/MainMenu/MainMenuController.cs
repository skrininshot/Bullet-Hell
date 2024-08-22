using System;
using UnityEngine.UI;
using Zenject;

public class MainMenuController : IInitializable, IDisposable
{
    private readonly SceneTransition _sceneTransition;
    private readonly Button _startButton;
    private readonly CursorController _cursorController;

    private int _selectedLevelBuildIndex = 2;

    public MainMenuController(SceneTransition sceneTransition, Button startButton, CursorController cursorController) 
    {
        _sceneTransition = sceneTransition;
        _startButton = startButton;
        _cursorController = cursorController;
    }

    public void Initialize()
    {
        _startButton.onClick.AddListener(StartLevel);
        _cursorController.RegisterUser(this);
    }


    public void Dispose()
    {
        _startButton.onClick.RemoveListener(StartLevel);
        _cursorController.UnregisterUser(this);
    }

    public void SelectLevel(int index)
    {
        _selectedLevelBuildIndex = index;
    }

    private void StartLevel()
    {
        _sceneTransition.TransitionToLevel(_selectedLevelBuildIndex);
    }
}