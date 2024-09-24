using UnityEngine.InputSystem;
using Zenject;

public class MouseAiming : MouseRotate
{
    private CameraZoom _camera;

    [Inject]
    private void Construct(CameraZoom cameraZoom)
    {
        _camera = cameraZoom;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerInput.PC.Zoom.performed += Zoom;
    }

    private void OnDisable()
    {
        _playerInput.PC.Zoom.performed -= Zoom;

        if (_camera.IsZoomed)
            _camera.ZoomOut();
    }

    protected override void Update()
    {
        RotationInput(_controlSettings.Sensitivity * _controlSettings.AimSensitivity);
    }

    private void Zoom(InputAction.CallbackContext context)
    {
        if (context.started)
            _camera.ZoomIn();

        if (context.canceled)
            _camera.ZoomOut();
    }
}
