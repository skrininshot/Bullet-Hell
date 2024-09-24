using DG.Tweening;
using UnityEngine;
using Zenject;
using System;

public class CameraMover : IInitializable, IDisposable, IPausable
{
    public Transform Transform { get; private set; }

    private readonly Camera _camera;
    private readonly PauseSystem _pauseSystem;

    private Sequence _moving;

    public CameraMover([Inject(Id = "MainCamera")] Camera camera, 
        PauseSystem pauseSystem) 
    {
        _camera = camera;
        Transform = _camera.transform;
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
    }

    public void Resume()
    {
        if (_moving.IsActive())
            _moving.Play();
    }

    public Tween SetTransform(Transform transform, float duration)
    {
        Transform.SetParent(transform, true);

        if (_moving.IsActive())
            _moving.Kill();

        _moving = DOTween.Sequence();
        _moving.Append(Transform.DOLocalMove(Vector3.zero, duration));
        _moving.Join(Transform.DOLocalRotate(Vector3.zero, duration));
        _moving.Play();

        return _moving;
    }
}