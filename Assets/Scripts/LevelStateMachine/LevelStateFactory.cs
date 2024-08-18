using ModestTree;

public enum LevelStates
{
    GettingReady,
    Playing,
    Score
}

public class LevelStateFactory : StateFactory
{
    readonly LevelStateGettingReady.Factory _gettingReadyFactory;
    readonly LevelStatePlaying.Factory _playingfactory;
    readonly LevelStateScore.Factory _scoreFactory;

    public LevelStateFactory(
        LevelStateGettingReady.Factory gettingReadyFactory,
        LevelStatePlaying.Factory playingFactory,
        LevelStateScore.Factory scoreFactory)
    {
        _gettingReadyFactory = gettingReadyFactory;
        _playingfactory = playingFactory;
        _scoreFactory = scoreFactory;
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
            case (int)LevelStates.Score:
                {
                    return _scoreFactory.Create();
                }

            default:
                {
                    throw Assert.CreateException();
                }      
        }
    }
}