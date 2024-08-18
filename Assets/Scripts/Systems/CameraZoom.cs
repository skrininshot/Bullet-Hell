using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class CameraZoom : IInitializable
{
    public bool IsZoomed { get; private set; }
    private readonly Settings _settings;
    private readonly Camera _camera;
    private float _defaultFOV;

    public CameraZoom (GameSettings gameSettings, Camera camera)
    {
        _settings  = gameSettings.CameraZoom;
        _camera = camera;
    }

    public void Initialize()
    {
        _defaultFOV = _camera.fieldOfView;
    }

    public void ZoomIn(float zoom)
    {
        IsZoomed = true;

        DOTween.Kill(_camera, true);

        float endValue = _camera.fieldOfView * zoom;
        DOTween.To(() => _camera.fieldOfView, x => _camera.fieldOfView = x, endValue, _settings.ZoomInDuration);
    }
    
    public void ZoomOut()
    {
        IsZoomed = false;

        DOTween.Kill(_camera, true);
        DOTween.To(() => _camera.fieldOfView, x => _camera.fieldOfView = x, _defaultFOV, _settings.ZoomOutDuration);
    }

    [Serializable]
    public class Settings
    {
        public float ZoomInDuration = 0.1f;
        public float ZoomOutDuration = 0.1f;
    }
}