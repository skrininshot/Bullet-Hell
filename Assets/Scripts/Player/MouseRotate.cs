using UnityEngine;
using Zenject;

public class MouseRotate : MonoBehaviour, IPausable
{
    [SerializeField] private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    [SerializeField] private RotationAxes _axes = RotationAxes.MouseXAndY;

    [SerializeField] private float _minimumX = -360f;
    [SerializeField] private float _maximumX = 360f;

    [SerializeField] private float _minimumY = -60f;
    [SerializeField] private float _maximumY = 60f;

    private float _rotationY = 0f;
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
        _rotationY = Mathf.Clamp(_rotationY, _minimumY, _maximumY);

        if (_axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + _mouseX;
            transform.localEulerAngles = new Vector3(-_rotationY, rotationX, 0);
        }
        else if (_axes == RotationAxes.MouseX)
            transform.Rotate(0, _mouseX, 0);
        else
            transform.localEulerAngles = new Vector3(-_rotationY, transform.localEulerAngles.y, 0);
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
