using DG.Tweening;
using System;
using UnityEngine;
using Zenject;

public class BulletHitAnimationOrbit : BulletHitAnimation, IPausable
{
    private readonly TimeShifter _timeShifter;
    private readonly CameraMover _cameraMover;
    private readonly Settings _settings;

    public BulletHitAnimationOrbit(TimeShifter timeShifter, CameraMover cameraMover, GameSettings settings)
    {
        _timeShifter = timeShifter;
        _cameraMover = cameraMover;
        _settings = settings.Bullet.Hit.Animations.Orbit;
    }

    public override void Play()
    {
        float segmentDuration = _settings.Duration / _settings.OrbitSegments;
        float segmentHeight = (_settings.OrbitRadius * 2f) / _settings.OrbitSegments;
        float segmentAngle = (360f / _settings.OrbitSegments);
        Vector3 originalPosition = Vector3.zero;

        _sequence.AppendCallback(() => _timeShifter.RegisterUser(this, _settings.TimeScale));

        //orbit rotation
        for (int i = 0; i < _settings.OrbitSegments; i++)
        {
            float angle = segmentAngle * i;
            
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

        _sequence.OnComplete(() => OnSequenceComplete());


        void CameraLookAtOriginalPosition() 
        {
            _cameraMover.Transform.localRotation =
                Quaternion.LookRotation(originalPosition - _cameraMover.Transform.localPosition);
        }       
    }

    protected override void OnSequenceComplete()
    {
        base.OnSequenceComplete();

        _timeShifter.UnregisterUser(this);
        _cameraMover.SetTransform(_cameraMover.Transform.parent, _settings.ExitDuration);
    }

    [Serializable]
    public class Settings
    {
        [Range(0f, 1f)] 
        public float Height = 1f;
        public float OrbitRadius = 0.125f;
        public int OrbitSegments = 3;
        public float EnterDuration = 1f;
        public float Duration = 3f;
        public float ExitDuration = 1f;
        public float TimeScale = 0.005f;
    }

    public class Factory : PlaceholderFactory<BulletHitAnimationOrbit> { }
}
