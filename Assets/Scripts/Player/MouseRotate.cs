using UnityEngine;
using Zenject;

public class MouseRotate : MonoBehaviour
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

    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _controlSettings = gameSettings.Player.Control;
    }

    private void Start()
    {
        _rotationY = -transform.localEulerAngles.x;
    }

    protected virtual void Update()
    {
        RotationInput();
    }

    protected virtual void LateUpdate()
    {
        Rotate(_controlSettings.Sensitivity);
    }

    private void RotationInput()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");
    }

    protected virtual void Rotate(float sensitivity)
    {
        _rotationY += _mouseY * sensitivity;
        _rotationY = Mathf.Clamp(_rotationY, _minimumY, _maximumY);

        if (_axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + _mouseX * _controlSettings.Sensitivity;
            transform.localEulerAngles = new Vector3(-_rotationY, rotationX, 0);
        }
        else if (_axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, _mouseX * _controlSettings.Sensitivity, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(-_rotationY, transform.localEulerAngles.y, 0);
        }
    }
}