using DG.Tweening;
using System;

public class BulletAnimationController : IPausable
{
    public bool AnimationIsPlaying => _currentAnimation != null && _currentAnimation.IsActive;
    public Action OnAnimationComplete;

    private readonly PauseSystem _pauseSystem;
    private readonly BulletHitAnimationFactory _animationFactory;
    private readonly CameraMover _cameraMover;
    private readonly Bullet _bullet;
    private readonly Settings _settings;

    private BulletHitAnimation _currentAnimation = null;
    private Tween _movingToBullet;

    public BulletAnimationController(PauseSystem pauseSystem,
        BulletHitAnimationFactory animationFactory, GameSettings gameSettings,
        CameraMover cameraMover, Bullet bullet)
    {
        _pauseSystem = pauseSystem;
        _animationFactory = animationFactory;
        _cameraMover = cameraMover;
        _settings = gameSettings.Bullet.Hit.Animations.Controller;
        _bullet = bullet;
    }
    public void Initialize()
    {
        CameraMoveToBullet();

        _pauseSystem.RegisterPausable(this);
    }

    public void Dispose()
    {
        _currentAnimation?.Dispose();
        _currentAnimation = null;
        _movingToBullet.Kill();
        _movingToBullet = null;

        _pauseSystem.UnregisterPausable(this);
    }

    public void Pause()
    {
        if (_movingToBullet.IsActive())
            _movingToBullet.Pause();
    }

    public void Resume()
    {
        if (_movingToBullet.IsActive())
            _movingToBullet.Play();
    }

    private Tween CameraMoveToBullet() =>
        _movingToBullet = _cameraMover.SetTransform(_bullet.CameraPoint, _settings.CameraMoveToBulletSpeed);
    
    public void PlayNewAnimation()
    {
        if (AnimationIsPlaying) return;

        if (_currentAnimation != null)
        {
            _currentAnimation.Dispose();
            _currentAnimation.OnComplete -= AnimationComplete;
        }

        _currentAnimation =
           _animationFactory.CreateAnimation((int)BulletHitAnimations.Orbit);

        _currentAnimation.Create();
        _currentAnimation.OnComplete += AnimationComplete;

        _currentAnimation.Play();
    }

    private void AnimationComplete()
    {
        CameraMoveToBullet();
        OnAnimationComplete?.Invoke();
    }

    [Serializable]
    public class Settings
    {
        public float CameraMoveToBulletSpeed = 0.5f;
    }
}