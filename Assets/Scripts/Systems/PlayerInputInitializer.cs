using System;
using Zenject;

public class PlayerInputInitializer : IInitializable, IDisposable
{
    [Inject] private PlayerInput _playerInput;

    public void Initialize() => _playerInput.Enable();

    public void Dispose() => _playerInput.Disable();
}
