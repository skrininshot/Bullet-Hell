using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class TimeShifter : IInitializable, IDisposable
{
    private Settings _settings;
    private float _timeScaleBeforePause = 1f;
    private bool _isPaused = false;
    private Tween _timeShiftTween = null;

    public TimeShifter(GameSettings gameSettings)
    {
        _settings = gameSettings.TimeShifter;
    }

    public void TimeShift(float value)
    {
        if (_isPaused) return;

        value = Mathf.Clamp01(value);
        var startTimescale = Time.timeScale;

        if (_timeShiftTween.IsActive())
            _timeShiftTween.Kill(true);

        _timeShiftTween = DOVirtual.Float(startTimescale, value, _settings.TimeShiftDuration, f =>
        {    
            SetTimeScale(f);
        });
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void SetIsPaused(bool value)
    {
        _isPaused = value;

        if (_isPaused)
        {
            if (_timeShiftTween.IsActive())
                _timeShiftTween.Pause();

            _timeScaleBeforePause = Time.timeScale;
            SetTimeScale(0f);
        }
        else
        {
            if (_timeShiftTween.IsActive())
                _timeShiftTween.Play();

            SetTimeScale(_timeScaleBeforePause);
        }
    }
 
    public void Reset() => SetTimeScale(_settings.DefaultTimeScale);

    public void Initialize()
    {
        Reset();
    }

    public void Dispose()
    {
        Reset();
    }

    [Serializable]
    public class Settings
    {
        public float DefaultTimeScale = 1f;
        public float TimeShiftDuration = 0.25f;
    }
}
