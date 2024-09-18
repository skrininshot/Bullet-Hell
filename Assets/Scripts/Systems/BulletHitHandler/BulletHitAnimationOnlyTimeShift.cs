using DG.Tweening;
using System;
using Zenject;

public class BulletHitAnimationOnlyTimeShift : BulletHitAnimation
{
    private readonly TimeShifter _timeShifter;
    private readonly Settings _settings;

    public BulletHitAnimationOnlyTimeShift(TimeShifter timeShifter, GameSettings settings)
    {
        _timeShifter = timeShifter;
        _settings = settings.Bullet.Hit.Animations.OnlyTimeShift;
    }

    public override void Play()
    {
        _sequence.AppendCallback(() => _timeShifter.RegisterUser(this, _settings.TimeShiftValue));
        _sequence.AppendInterval(_settings.TimeShiftValue);
        _sequence.AppendInterval(_settings.TimeShiftDelay);

        _sequence.OnComplete(() => OnSequenceComplete());

        _sequence.Play();
    }

    protected override void OnSequenceComplete()
    {
        base.OnSequenceComplete();

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
