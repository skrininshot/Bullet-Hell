using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class AirFlowEffectSpawner : IInitializable, IDisposable, IPausable
{
    private GameSettings.BulletSettings _settings;
    private PauseSystem _pauseSystem;
    private AirFlowEffect.Factory _factory;
    private Bullet _bullet;

    private float _sizeMultiply = 0f;

    private Sequence _sequence;

    public AirFlowEffectSpawner(GameSettings gameSettings, PauseSystem pauseSystem,
        AirFlowEffect.Factory factory, Bullet bullet)
    {
        _settings = gameSettings.Bullet;
        _pauseSystem = pauseSystem;
        _factory = factory;
        _bullet = bullet;
    }

    public void Initialize()
    {
        _pauseSystem.RegisterPausable(this);
    }

    public void Dispose()
    {
        _pauseSystem.UnregisterPausable(this);
    }

    public void Start()
    {
        CreateSequence();
    }

    public void Stop()
    {
        if (_sequence.IsActive())
            _sequence.Kill();

        _sequence = null;
    }

    public void Pause()
    {
        if (_sequence.IsActive())
            _sequence.Pause();
    }

    public void Resume()
    {
        if (_sequence.IsActive())
            _sequence.Play();
    }

    private void CreateSequence()
    {
        _sequence = DOTween.Sequence();

        _sequence.PrependInterval(_settings.AirFlow.Frequency);
        _sequence.AppendCallback(() => Spawn());

        _sequence.SetLoops(-1, LoopType.Restart);
        _sequence.SetUpdate(UpdateType.Fixed, false);
    }

    private void CountSizeMultiply()
    {
        _sizeMultiply = Mathf.Abs(Mathf.Sin(Time.time * _settings.AirFlow.SinMultiply));
    }

    private void Spawn()
    {
        CountSizeMultiply();
        _factory.Create(_bullet.Transform.position, _bullet.Transform.eulerAngles, _settings.AirFlow.Effect, _sizeMultiply);
    }
}