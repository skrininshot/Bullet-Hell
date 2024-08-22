using UnityEngine;
using NaughtyAttributes;
using Zenject;

public class MouseRotate : MonoBehaviour, IPausable
{
    [SerializeField] private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    [SerializeField] private RotationAxes _axes = RotationAxes.MouseXAndY;

    [SerializeField] private bool _clampX;
    [ShowIf(nameof(_clampX))][SerializeField] private float _minimumX = -360f;
    [ShowIf(nameof(_clampX))][SerializeField] private float _maximumX = 360f;

    [SerializeField] private bool _clampY;
    [ShowIf(nameof(_clampY))][SerializeField] private float _minimumY = -60f;
    [ShowIf(nameof(_clampY))][SerializeField] private float _maximumY = 60f;

    private float _rotationY = 0f;
    private float _rotationX = 0f;
    private float _mouseX = 0f;
    private float _mouseY = 0f;

    protected GameSettings.PlayerSetttings.ControlSettings _controlSettings;
    protected PauseSystem _pauseSystem;

    protected bool _isPaused;

    [Inject]
    private void Construct(GameSettings gameSettings, PauseSystem pauseSystem)
    {
        _controlSettings = gameSettings.Player.Control;
        _pauseSystem = pauseSystem;
    }

    private void Start()
    {
        _rotationY = -transform.localEulerAngles.x;
        _rotationX = transform.localEulerAngles.y;
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

        _mouseX = Input.GetAxis("Mouse X") * sensitivity;
        _mouseY = Input.GetAxis("Mouse Y") * sensitivity;
    }

    private void Rotate()
    {
        if (_isPaused) return;

        _rotationY += _mouseY;
        if (_clampY) 
            _rotationY = Mathf.Clamp(_rotationY, _minimumY, _maximumY);

        _rotationX += _mouseX;
        if (_clampX) 
            _rotationX = Mathf.Clamp(_rotationX, _minimumX, _maximumX);

        if (_axes == RotationAxes.MouseXAndY)
            transform.localEulerAngles = new (-_rotationY, _rotationX, 0);
        else if (_axes == RotationAxes.MouseX)
            transform.Rotate(0, _rotationX, 0);
        else
            transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
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
