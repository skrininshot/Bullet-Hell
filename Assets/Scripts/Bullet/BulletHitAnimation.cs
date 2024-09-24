using DG.Tweening;
using System;
using Zenject;

public class BulletHitAnimation : IPausable
{
    public bool IsActive => _sequence != null && _sequence.IsActive();
    public Action OnComplete;

    protected Sequence _sequence;

    [Inject] private PauseSystem _pauseSystem;

    public virtual void Play() { }

    public void Create()
    {
        _sequence = DOTween.Sequence();
        _sequence.OnStart(OnSequenceStart);
    }

    public void Dispose()
    {
        _sequence.Kill(true);
    }

    protected virtual void OnSequenceStart()
    {
        _pauseSystem.RegisterPausable(this);
    }

    protected virtual void OnSequenceComplete()
    {
        _pauseSystem.UnregisterPausable(this);
        OnComplete?.Invoke();
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
}
