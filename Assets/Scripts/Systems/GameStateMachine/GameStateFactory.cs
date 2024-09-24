using ModestTree;

public enum GameStates
{
    Transition,
    Menu,
    Play
}

public class GameStateFactory : StateFactory
{
    readonly GameStateTransition.Factory _transitionFactory;
    readonly GameStateMenu.Factory _menuFactory;
    readonly GameStatePlay.Factory _playFactory;

    public GameStateFactory(
        GameStateTransition.Factory transitionFactory,
        GameStateMenu.Factory menuFactory,
        GameStatePlay.Factory playFactory)
    {
        _transitionFactory = transitionFactory;
        _menuFactory = menuFactory;
        _playFactory = playFactory;
    }

    public override State CreateState(int state)
    {
        switch (state)
        {
            case (int)GameStates.Transition:
                {
                    return _transitionFactory.Create();
                }
            case (int)GameStates.Menu:
                {
                    return _menuFactory.Create();
                }
            case (int)GameStates.Play:
                {
                    return _playFactory.Create();
                }

            default:
                {
                    throw Assert.CreateException();
                }
        }
    }
}