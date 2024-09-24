using UnityEngine;
using Zenject;

public class GameSettingsInstaller : MonoInstaller
{
    [SerializeField] private GameSettings _gameSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(_gameSettings).AsSingle();
    }
}
