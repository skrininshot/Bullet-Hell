using ModestTree;

public enum LevelStates
{
    GettingReady,
    Playing,
    Award
}

public class LevelStateFactory : StateFactory
{
    readonly LevelStateGettingReady.Factory _gettingReadyFactory;
    readonly LevelStatePlaying.Factory _playingfactory;
    readonly LevelStateAward.Factory _awardFactory;

    public LevelStateFactory(
        LevelStateGettingReady.Factory gettingReadyFactory,
        LevelStatePlaying.Factory playingFactory,
        LevelStateAward.Factory awardFactory)
    {
        _gettingReadyFactory = gettingReadyFactory;
        _playingfactory = playingFactory;
        _awardFactory = awardFactory;
    }

    public override State CreateState(int state)
    {
        switch (state)
        {
            case (int)LevelStates.GettingReady:
                {
                    return _gettingReadyFactory.Create();
                }
            case (int)LevelStates.Playing:
                {
                    return _playingfactory.Create();
                }
            case (int)LevelStates.Award:
                {
                    return _awardFactory.Create();
                }

            default:
                {
                    throw Assert.CreateException();
                }      
        }
    }
}