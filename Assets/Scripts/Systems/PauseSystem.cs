using System;
using UnityEngine;
using Zenject;

public class PauseSystem : IInitializable, ITickable, IDisposable
{
    public bool IsPaused { get; private set; }

    private readonly PauseSystemView _pauseSystemView;
    private readonly SceneTransition _sceneTransition;
    private readonly TimeShifter _timeShifter;

    public PauseSystem(PauseSystemView pauseSystemView, SceneTransition sceneTransition, TimeShifter timeShifter)
    {
        _pauseSystemView = pauseSystemView;
        _sceneTransition = sceneTransition;
        _timeShifter = timeShifter;
    }

    public void Initialize()
    {
        _pauseSystemView.ContinueButton.onClick.AddListener(Pause);
        _pauseSystemView.MenuButton.onClick.AddListener(_sceneTransition.TransitionToMenu);
    }

    private void Pause()
    {
        IsPaused = !IsPaused;
        _pauseSystemView.Pause(IsPaused);

        //if (IsPaused)
            //_timeShifter.PauseTime(); - pause
        //else
            //_timeShifter.ResetTimeScale(); - unpause
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Dispose()
    {
        _pauseSystemView.ContinueButton.onClick.RemoveListener(Pause);
        _pauseSystemView.MenuButton.onClick.RemoveListener(_sceneTransition.TransitionToMenu);
    }
}
