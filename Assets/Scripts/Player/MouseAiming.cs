using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class MouseAiming : MouseRotate
{
    [HideInInspector] public UnityEvent OnClick = new();

    private CameraZoom _camera;

    [Inject]
    private void Construct(CameraZoom cameraZoom)
    {
        _camera = cameraZoom;
    }

    private void OnDisable()
    {
        if (_camera.IsZoomed)
            _camera.ZoomOut();
    }

    protected override void Update()
    {
        HandleMouseButtons();
        RotationInput(_controlSettings.Sensitivity * _controlSettings.AimSensitivity);
    }

    private void HandleMouseButtons()
    {
        if (_isPaused) return;
        
        bool lmbClicked = Input.GetMouseButtonDown(0);
        bool rmbClicked = Input.GetMouseButton(1);

        if (lmbClicked)
        {
            OnClick?.Invoke();

            return;
        }

        if (_camera.IsZoomed != rmbClicked)
            if (rmbClicked)
                _camera.ZoomIn();
            else
                _camera.ZoomOut();
    }
}
