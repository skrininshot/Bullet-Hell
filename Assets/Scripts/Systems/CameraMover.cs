using DG.Tweening;
using UnityEngine;

public class CameraMover
{
    private readonly Camera _camera;

    public CameraMover(Camera camera) 
    {
        _camera = camera;
    }

    public void SetTransform(Transform transform, float speed)
    {
        _camera.transform.SetParent(transform);
        _camera.transform.DOKill(true);
        _camera.transform.DOLocalMove(Vector3.zero, speed);
        _camera.transform.DOLocalRotate(Vector3.zero, speed);
    }
}