using DG.Tweening;
using UnityEngine;

public class CameraMover
{
    private readonly Camera _camera;

    private Tween _moving;
    private Tween _rotation;

    public CameraMover(Camera camera) 
    {
        _camera = camera;
    }

    public void SetTransform(Transform transform, float speed)
    {
        _camera.transform.SetParent(transform);

        if (_moving.IsActive()) _moving.Kill();
        _moving = _camera.transform.DOLocalMove(Vector3.zero, speed);

        if (_rotation.IsActive()) _rotation.Kill();
        _rotation = _camera.transform.DOLocalRotate(Vector3.zero, speed);
    }
}