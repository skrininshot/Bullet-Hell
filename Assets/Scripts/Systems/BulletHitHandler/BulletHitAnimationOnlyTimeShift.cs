using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class BulletHitAnimationOnlyTimeShift : BulletHitAnimation
{
    private readonly TimeShifter _timeShifter;
    private readonly Settings _settings;
    private Sequence _sequence;

    public BulletHitAnimationOnlyTimeShift(TimeShifter timeShifter, GameSettings settings)
    {
        _timeShifter = timeShifter;
        _settings = settings.Bullet.Hit.Animations.OnlyTimeShift;
    }

    public override void Play()
    {
        _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() => _timeShifter.RegisterUser(this, _settings.TimeShiftValue));
        _sequence.AppendInterval(_settings.TimeShiftValue);
        _sequence.AppendInterval(_settings.TimeShiftDelay);
        _sequence.AppendCallback(() => _timeShifter.UnregisterUser(this));
        _sequence.Play();
    }

    public override void Dispose()
    {
        _sequence.Kill();
        _timeShifter.UnregisterUser(this);
    }

    [Serializable]
    public class Settings
    {
        public float TimeShiftValue = 0.01f;
        public float TimeShiftDelay = 1f;
    }

    public class Factory : PlaceholderFactory<BulletHitAnimationOnlyTimeShift> { }
}
