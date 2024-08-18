using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class CameraMover : IInitializable, IDisposable
{
    private readonly Camera _camera;

    public CameraMover(Camera camera) 
    {
        _camera = camera;
    }

    public void Initialize()
    {
        
    }

    public void Dispose()
    {
        
    }

    public void SetTransform(Transform transform, float speed)
    {
        _camera.transform.SetParent(transform);
        _camera.transform.DOKill(true);
        _camera.transform.DOLocalMove(Vector3.zero, speed);
    }
}