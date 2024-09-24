using UnityEngine;
using NaughtyAttributes;
using Zenject;
using UnityEngine.InputSystem;

public class MouseRotate : MonoBehaviour, IPausable
{
    [SerializeField] private bool _clampX;
    [ShowIf(nameof(_clampX))][SerializeField] private float _minimumX = -360f;
    [ShowIf(nameof(_clampX))][SerializeField] private float _maximumX = 360f;

    [SerializeField] private bool _clampY;
    [ShowIf(nameof(_clampY))][SerializeField] private float _minimumY = -60f;
    [ShowIf(nameof(_clampY))][SerializeField] private float _maximumY = 60f;

    protected PlayerInput _playerInput;
    private Transform _transform;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private float _mouseX = 0f;
    private float _mouseY = 0f;

    protected GameSettings.PlayerSetttings.ControlSettings _controlSettings;
    protected PauseSystem _pauseSystem;

    protected bool _isPaused;

    [Inject]
    private void Construct(GameSettings gameSettings, PlayerInput playerInput, PauseSystem pauseSystem)
    {
        _controlSettings = gameSettings.Player.Control;
        _playerInput = playerInput;
        _pauseSystem = pauseSystem;
    }

    private void Awake()
    {
        _transform = transform;
    }

    protected virtual void OnEnable()
    {
        _rotationX = _transform.eulerAngles.x;
        _rotationY = _transform.eulerAngles.y;

        _pauseSystem.RegisterPausable(this);
    }

    private void OnDestroy()
    {
        _pauseSystem.UnregisterPausable(this);
    }

    protected virtual void Update() => RotationInput(_controlSettings.Sensitivity);

    protected virtual void LateUpdate() => Rotate();

    protected void RotationInput(float sensitivity)
    {
        if (_isPaused) return;

        Vector2 mouseDeltaInput = _playerInput.PC.Point.ReadValue<Vector2>();

        if (mouseDeltaInput == null) return;

        _mouseX = mouseDeltaInput.x * sensitivity;
        _mouseY = mouseDeltaInput.y * sensitivity;
    }

    private void Rotate()
    {
        if (_isPaused) return;

        _rotationX += -_mouseY;

        if (_rotationX > 270)
            _rotationX -= 360;

        if (_clampX) 
            _rotationX = Mathf.Clamp(_rotationX, _minimumX, _maximumX);

        _rotationY += _mouseX;

        if (_rotationY > 270)
            _rotationY -= 360;

        if (_clampY) 
            _rotationY = Mathf.Clamp(_rotationY, _minimumY, _maximumY);

         _transform.eulerAngles = new (_rotationX, _rotationY, 0);
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }
}
