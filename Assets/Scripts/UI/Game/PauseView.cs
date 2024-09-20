using UnityEngine;
using UnityEngine.UI;

public class PauseView : PauseBaseView
{
    public Button ContinueButton => _continueButton;
    [SerializeField] private Button _continueButton;
}