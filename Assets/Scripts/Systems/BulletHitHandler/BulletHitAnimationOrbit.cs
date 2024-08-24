using Zenject;

public class BulletHitAnimationOrbit : BulletHitAnimation
{
    private readonly TimeShifter _timeShifter;
    private readonly CameraMover _cameraMover;

    public BulletHitAnimationOrbit(TimeShifter timeShifter, CameraMover cameraMover)
    {
        _timeShifter = timeShifter;
        _cameraMover = cameraMover;
    }

    public override void Play()
    {

    }

    public class Factory : PlaceholderFactory<BulletHitAnimationOrbit> { }
}
