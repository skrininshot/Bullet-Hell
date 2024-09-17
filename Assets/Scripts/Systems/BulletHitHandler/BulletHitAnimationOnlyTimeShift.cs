using DG.Tweening;
using System;
using Zenject;

public class BulletHitAnimationOnlyTimeShift : BulletHitAnimation, IInitializable, IDisposable, IPausable
{
    private readonly TimeShifter _timeShifter;
    private readonly Settings _settings;
    private Sequence _sequence;
    private PauseSystem _pauseSystem;

    public BulletHitAnimationOnlyTimeShift(TimeShifter timeShifter, GameSettings settings, PauseSystem pauseSystem)
    {
        _timeShifter = timeShifter;
        _settings = settings.Bullet.Hit.Animations.OnlyTimeShift;
        _pauseSystem = pauseSystem;
    }
    public void Initialize()
    {
        _pauseSystem.RegisterPausable(this);
    }

    public override void Dispose()
    {
        _sequence.Kill(true);
        _pauseSystem.UnregisterPausable(this);
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

    public override void Play()
    {
        _sequence = DOTween.Sequence();

        _sequence.AppendCallback(() => _timeShifter.RegisterUser(this, _settings.TimeShiftValue));
        _sequence.AppendInterval(_settings.TimeShiftValue);
        _sequence.AppendInterval(_settings.TimeShiftDelay);

        _sequence.OnComplete(() => 
        { 
            _timeShifter.UnregisterUser(this); 
        });

        _sequence.Play();
    }

    [Serializable]
    public class Settings
    {
        public float TimeShiftValue = 0.01f;
        public float TimeShiftDelay = 1f;
    }

    public class Factory : PlaceholderFactory<BulletHitAnimationOnlyTimeShift> { }
}
