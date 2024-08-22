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

    public void SetTransform(Transform transform, float duration)
    {
        Debug.Log("set transform");
        _camera.transform.SetParent(transform, true);

        if (_moving.IsActive()) _moving.Kill();
        _moving = _camera.transform.DOLocalMove(Vector3.zero, duration)
            .OnUpdate(() => Debug.Log($"_camera.transform.localPosition: {_camera.transform.localPosition}")).SetUpdate(true);

        if (_rotation.IsActive()) _rotation.Kill();
        _rotation = _camera.transform.DOLocalRotate(Vector3.zero, duration)
            .SetUpdate(true);
    }
}