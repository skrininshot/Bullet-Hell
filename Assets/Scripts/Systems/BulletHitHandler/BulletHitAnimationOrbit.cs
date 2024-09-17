using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class BulletHitAnimationOrbit : BulletHitAnimation, IInitializable, IDisposable, IPausable
{
    private readonly TimeShifter _timeShifter;
    private readonly CameraMover _cameraMover;
    private readonly Settings _settings;
    private Sequence _sequence;
    private PauseSystem _pauseSystem;
    
    public BulletHitAnimationOrbit(TimeShifter timeShifter, CameraMover cameraMover, GameSettings settings, PauseSystem pauseSystem)
    {
        _timeShifter = timeShifter;
        _cameraMover = cameraMover;
        _settings = settings.Bullet.Hit.Animations.Orbit;
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

    public override void Play()
    {
        _sequence = DOTween.Sequence();

        float segmentDuration = _settings.Duration / _settings.OrbitSegments;
        float segmentHeight = (_settings.OrbitRadius * 2f) / _settings.OrbitSegments;
        Vector3 originalPosition = _cameraMover.Transform.localPosition;
        Vector3 startPosition = originalPosition + new Vector3(0f, _settings.OrbitRadius * _settings.Height, 0f);

        _sequence.AppendCallback(() => _timeShifter.RegisterUser(this, _settings.TimeScale));

        //orbit rotation
        for (int i = 0; i < _settings.OrbitSegments; i++)
        {
            float angle = (360f / _settings.OrbitSegments) * i;
            
            Vector3 orbitPosition = 
                new (
                    Mathf.Cos(angle * Mathf.Deg2Rad) * _settings.OrbitRadius,
                    (_settings.OrbitRadius - segmentHeight * i) * _settings.Height,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * _settings.OrbitRadius);

            if (i == 0)
            {
                _sequence.Append(_cameraMover.Transform
                    .DOLocalMove(originalPosition + orbitPosition, _settings.EnterDuration));

                Quaternion lookRotation = Quaternion.LookRotation(originalPosition - orbitPosition);

                _sequence.Join(_cameraMover.Transform
                    .DOLocalRotateQuaternion(lookRotation, _settings.EnterDuration));
            }  
            else
                _sequence.Append(_cameraMover.Transform
                    .DOLocalMove(originalPosition + orbitPosition, segmentDuration)
                    .OnUpdate(() => CameraLookAtOriginalPosition()));
        }

        //return to original
        _sequence.Append(_cameraMover.Transform
            .DOLocalMove(originalPosition, _settings.ExitDuration));

        _sequence.Join(_cameraMover.Transform
                .DOLocalRotateQuaternion(Quaternion.LookRotation(Vector3.zero), _settings.ExitDuration));

        _sequence.OnComplete(() =>
        {
            _timeShifter.UnregisterUser(this);
            _cameraMover.Transform.localPosition = Vector3.zero;
            _cameraMover.Transform.localEulerAngles = Vector3.zero;
        });


        void CameraLookAtOriginalPosition() 
        {
            _cameraMover.Transform.localRotation =
                Quaternion.LookRotation(originalPosition - _cameraMover.Transform.localPosition);
        }       
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

    [Serializable]
    public class Settings
    {
        [Range(0f, 1f)] 
        public float Height = 0.5f;
        public float OrbitRadius = 0.1f;
        public int OrbitSegments = 36;
        public float EnterDuration = 0.1f;
        public float Duration = 2f;
        public float ExitDuration = 0.1f;
        public float TimeScale = 0.01f;
    }

    public class Factory : PlaceholderFactory<BulletHitAnimationOrbit> { }
}
