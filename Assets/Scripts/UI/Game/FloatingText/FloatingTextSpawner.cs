using System;
using UnityEngine;
using Zenject;

public class FloatingTextSpawner
{
    private readonly FloatingText.Settings _settings;
    private readonly FloatingText.Factory _factory;
    private readonly Transform _cameraTransform;

    public FloatingTextSpawner(
        GameSettings gameSettings,
        FloatingText.Factory factory,
        [Inject(Id = "MainCamera")] Camera camera)
    {
        _settings = gameSettings.UI.Game.FloatingText;
        _factory = factory;
        _cameraTransform = camera.transform;
    }

    public void Spawn(Vector3 position, string value)
    {
        _factory.Create(position, value, _settings, _cameraTransform);
    }
}