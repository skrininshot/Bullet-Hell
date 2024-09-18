using DG.Tweening;
using UnityEngine;
using Zenject;
using System;

public class CameraMover : IInitializable, IDisposable, IPausable
{
    public Transform Transform { get; private set; }
    private readonly Camera _camera;

    private Tween _moving;
    private Tween _rotation;

    private PauseSystem _pauseSystem;

    public CameraMover(Camera camera, PauseSystem pauseSystem) 
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
        Transform.SetParent(transform, true);

        if (_moving.IsActive()) _moving.Kill();
        _moving = Transform.DOLocalMove(Vector3.zero, duration);

        if (_rotation.IsActive()) _rotation.Kill();
        _rotation = Transform.DOLocalRotate(Vector3.zero, duration);
    }
}