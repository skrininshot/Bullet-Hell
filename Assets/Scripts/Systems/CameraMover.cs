using DG.Tweening;
using UnityEngine;
using static GameSettings.PlayerSetttings;
using Zenject;
using System;

public class CameraMover : IInitializable, IDisposable, IPausable
{
    private readonly Camera _camera;

    private Tween _moving;
    private Tween _rotation;

    private PauseSystem _pauseSystem;

    public CameraMover(Camera camera, PauseSystem pauseSystem) 
    {
        _camera = camera;
        _pauseSystem = pauseSystem;
    }

    public void Initialize()
    {
        _pauseSystem.RegisterPausable(this);
    }

    public void Dispose()
    {
        _pauseSystem.UnregisterPausable(this);
    }

    public void Pause()
    {
        if (_moving.IsActive())
            _moving.Pause();

        if (_rotation.IsActive())
            _rotation.Pause();
    }

    public void Resume()
    {
        if (_moving.IsActive())
            _moving.Play();

        if (_rotation.IsActive())
            _rotation.Play();
    }

    public void SetTransform(Transform transform, float duration)
    {
        _camera.transform.localPosition = Vector3.zero;
        _camera.transform.localEulerAngles = Vector3.zero;

        _camera.transform.SetParent(transform, true);

        if (_moving.IsActive()) _moving.Kill();
        _moving = _camera.transform.DOLocalMove(Vector3.zero, duration);

        if (_rotation.IsActive()) _rotation.Kill();
        _rotation = _camera.transform.DOLocalRotate(Vector3.zero, duration);
    }
}