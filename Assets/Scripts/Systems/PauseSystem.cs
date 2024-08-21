using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseSystem : IInitializable, ITickable, IDisposable
{
    public bool IsPaused { get; private set; }

    private readonly PauseView _pauseView;
    private readonly SceneTransition _sceneTransition;
    private readonly TimeShifter _timeShifter;
    private readonly List<IPausable> _pausables = new ();

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

        if (IsPaused)
            foreach (var pausable in _pausables)
                pausable.Pause();
        else
            foreach (var pausable in _pausables)
                pausable.Resume();
    }

    public void RegisterPausable(IPausable obj) => _pausables.Add(obj);

    public void UnregisterPausable(IPausable obj) => _pausables.Remove(obj);

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
