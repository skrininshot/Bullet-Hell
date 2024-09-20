using UnityEngine;

public class BulletHitHandler
{
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly BulletHitAnimationController _animationController;
    private readonly LevelScoreRecorder _levelScoreRecorder;
    private readonly Bullet _bullet;

    public BulletHitHandler(PlayerStateMachine playerStateMachine, 
        BulletHitAnimationController animator, LevelScoreRecorder levelScoreRecorder, 
        Bullet bullet)
    {
        _playerStateMachine = playerStateMachine;
        _animationController = animator;
        _levelScoreRecorder = levelScoreRecorder;
        _bullet = bullet;
    }

    public void Initialize()
    {
        _animationController.Initialize(ReturnToAimingState);
        _bullet.OnHit.AddListener(HandleHit);
    }

    public void Dispose()
    {
        _animationController.Dispose();
        _bullet.OnHit.RemoveListener(HandleHit);
    }

    public void HandleHit(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage();
            _levelScoreRecorder.AddToScore(damagable);
            _animationController.PlayNewAnimation();
        }
        else
        {
            _animationController.InvokeCompleteAction(); 
        }
    }

    private void ReturnToAimingState() => 
        _playerStateMachine.ChangeState((int)PlayerStates.Aiming);
}
