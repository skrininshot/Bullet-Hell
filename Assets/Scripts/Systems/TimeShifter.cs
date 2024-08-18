using DG.Tweening;
using System;
using UnityEngine;

public class TimeShifter
{
    private Settings _settings;

    public TimeShifter(GameSettings gameSettings)
    {
        _settings = gameSettings.TimeShifter;
    }

    public void SetTimeScale(float value)
    {
        value = Mathf.Clamp01(value);
        var startTimescale = Time.timeScale;

        DOTween.Kill(this);

        DOVirtual.Float(startTimescale, value, _settings.TimeShiftDuration, f =>
        {
            Time.timeScale = f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        });
    }

    [Serializable]
    public class Settings
    {
        public float TimeShiftDuration = 0.25f;
    }
}
