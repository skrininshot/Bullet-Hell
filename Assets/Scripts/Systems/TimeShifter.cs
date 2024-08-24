using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class TimeShifter : IInitializable, IDisposable
{
    private Settings _settings;
    private bool _isPaused = false;
    private Tween _timeShiftTween = null;

    private Dictionary<object, float> _users = new();

    public TimeShifter(GameSettings gameSettings)
    {
        _settings = gameSettings.TimeShifter;
    }

    public void RegisterUser(object user, float timeScale)
    {
        if (_users.Keys.Contains(user)) return;

        _users.Add(user, timeScale);
        HandleUsers();
    }

    public void UnregisterUser(object user)
    {
        if (!_users.Keys.Contains(user)) return;
        
        _users.Remove(user);
        HandleUsers();
    }

    private void HandleUsers()
    {
        if (_users.Count > 0)
        {
            float minimalTimeScale = _users.Values.Min();
            TimeShift(minimalTimeScale);
        }
        else
            TimeShift(1f);
    }

    private void TimeShift(float value)
    {
        if (_isPaused) return;

        value = Mathf.Clamp01(value);

        if (Time.timeScale == value) return;

        if (_timeShiftTween.IsActive())
            _timeShiftTween.Kill();

        var startTimescale = Time.timeScale;

        _timeShiftTween = DOVirtual.Float(startTimescale, value, _settings.TimeShiftDuration, f =>
        {    
            SetTimeScale(f);
        });
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Debug.Log($"Time.timeScle = {value}");

    }

    private void Reset()
    {
        if (_timeShiftTween.IsActive())
            _timeShiftTween.Kill();

        SetTimeScale(_settings.DefaultTimeScale);
    }

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
