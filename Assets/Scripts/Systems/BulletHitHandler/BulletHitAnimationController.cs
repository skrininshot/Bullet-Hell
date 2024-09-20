using System;

public class BulletHitAnimationController
{
    private readonly BulletHitAnimationFactory _animationFactory;

    private BulletHitAnimation _currentAnimation;
    private Action _actionOnComplete;

    private bool _invokeCompleteAction = false;

    public BulletHitAnimationController(BulletHitAnimationFactory animationFactory)
    {
        _animationFactory = animationFactory;
    }

    public void Initialize(Action actionOnComplete)
    {
        CreateNewAnimation();
        _actionOnComplete = actionOnComplete;
    }

    public void Dispose() => DisposeCurrentAnimation();

    public void PlayNewAnimation()
    {
        if (_currentAnimation.IsActive) return;

        DisposeCurrentAnimation();

        CreateNewAnimation();

        _currentAnimation.Play();
    }

    public void InvokeCompleteAction()
    {
        if (_currentAnimation.IsActive)
            _invokeCompleteAction = true;
        else
            _actionOnComplete?.Invoke();
    }

    private void CreateNewAnimation()
    {
        _currentAnimation =
           _animationFactory.CreateAnimation((int)BulletHitAnimations.Orbit);

        _currentAnimation.Create();
        _currentAnimation.OnComplete += OnAnimationComplete;
    }

    private void DisposeCurrentAnimation()
    {
        if (_currentAnimation == null) return;

        _currentAnimation?.Dispose();
        _currentAnimation.OnComplete -= OnAnimationComplete;
    }

    private void OnAnimationComplete()
    {
        if (!_invokeCompleteAction) return;

        _invokeCompleteAction = false;
        _actionOnComplete?.Invoke();
    }
}