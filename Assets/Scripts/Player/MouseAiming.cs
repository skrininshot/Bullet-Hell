using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class MouseAiming : MouseRotate
{
    [HideInInspector] public UnityEvent OnClick = new();

    [SerializeField] private float _aimZoomSize = 0.5f;

    private CameraZoom _camera;

    [Inject]
    private void Construct(CameraZoom cameraZoom)
    {
        _camera = cameraZoom;
    }

    protected override void Update()
    {
        float sensitivity = _controlSettings.Sensitivity * (_camera.IsZoomed ? _controlSettings.AimSensitivity : 1f );

        HandleMouseInput(sensitivity);
    }

    private void OnDisable()
    {
        _camera.ZoomOut();
    }

    protected override void HandleMouseInput(float sensitivity)
    {
        base.HandleMouseInput(sensitivity);

        bool lmbClicked = Input.GetMouseButtonDown(0);
        bool rmbClicked = Input.GetMouseButton(1);

        if (lmbClicked) 
        {
            OnClick?.Invoke();

            return;
        }

        if (_camera.IsZoomed != rmbClicked)
            if (rmbClicked)
                _camera.ZoomIn(_aimZoomSize);
            else
                _camera.ZoomOut();  
    }
}
