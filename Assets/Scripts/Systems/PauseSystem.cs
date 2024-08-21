using System;
using UnityEngine;
using Zenject;

public class PauseSystem : IInitializable, ITickable, IDisposable
{
    public bool IsPaused { get; private set; }

    private readonly PauseView _pauseView;
    private readonly SceneTransition _sceneTransition;
    private readonly TimeShifter _timeShifter;

    public PauseSystem(PauseView pauseSystemView, SceneTransition sceneTransition, TimeShifter timeShifter)
    {
        _pauseView = pauseSystemView;
        _sceneTransition = sceneTransition;
        _timeShifter = timeShifter;
    }

    public void Initialize()
    {
        _pauseView.ContinueButton.onClick.AddListener(Pause);
        _pauseView.MenuButton.onClick.AddListener(_sceneTransition.TransitionToMenu);
    }

    private void Pause()
    {
        IsPaused = !IsPaused;
        _pauseView.Pause(IsPaused);
        _timeShifter.SetIsPaused(IsPaused);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Dispose()
    {
        _pauseView.ContinueButton.onClick.RemoveListener(Pause);
        _pauseView.MenuButton.onClick.RemoveListener(_sceneTransition.TransitionToMenu);
    }
}
