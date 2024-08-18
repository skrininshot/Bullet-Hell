using UnityEngine.SceneManagement;
using Zenject;

public class SceneTransition : IInitializable
{

    private GameStateMachine _gameStateMachine;
    private GameStates _transitionNextState;

    public SceneTransition (GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void Initialize()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "SplashScene")
            TransitionToMenu();
        else if (currentScene.name == "MainMenu")
            _gameStateMachine.ChageState((int)GameStates.Menu);
        else if (currentScene.name.Contains("Level_"))
            _gameStateMachine.ChageState((int)GameStates.Play);
    }

    public void TransitionToLevel(int index)
    {
        _transitionNextState = GameStates.Play;
        _gameStateMachine.ChageState((int)GameStates.Transition);

        SceneManager.LoadScene(index);
        SceneManager.sceneLoaded += SceneLoaded;
    }

    protected void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _gameStateMachine.ChageState((int)_transitionNextState);
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    public void TransitionToMenu()
    {
        _transitionNextState = GameStates.Menu;
        _gameStateMachine.ChageState((int)GameStates.Transition);

        SceneManager.LoadScene("MainMenu");
        SceneManager.sceneLoaded += SceneLoaded;
    }
}