using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private int _sceneBuildIndex;

    private MainMenuController _mainMenuHandler;
    private Button _button;

    [Inject]
    private void Construct(MainMenuController mainMenuHandler)
    {
        _mainMenuHandler = mainMenuHandler;
    }

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonPressed);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(ButtonPressed);
    }

    private void ButtonPressed()
    {
        _mainMenuHandler.SelectLevel(_sceneBuildIndex);
    }
}