using UnityEngine;
using Zenject;

public class MouseRotate : MonoBehaviour
{
    [SerializeField] private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    [SerializeField] private RotationAxes _axes = RotationAxes.MouseXAndY;

    [SerializeField] private float _minimumX = -360F;
    [SerializeField] private float _maximumX = 360F;

    [SerializeField] private float _minimumY = -60F;
    [SerializeField] private float _maximumY = 60F;

    private float rotationY = 0F;

    protected GameSettings.PlayerSetttings.ControlSettings _controlSettings;

    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _controlSettings = gameSettings.Player.Control;
    }

    private void Start()
    {
        rotationY = -transform.localEulerAngles.x;
    }

    protected virtual void Update()
    {
        HandleMouseInput(_controlSettings.Sensitivity);
    }

    protected virtual void HandleMouseInput(float sensitivity)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationY += mouseY * sensitivity;
        rotationY = Mathf.Clamp(rotationY, _minimumY, _maximumY);

        if (_axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + mouseX * _controlSettings.Sensitivity;
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (_axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, mouseX * _controlSettings.Sensitivity, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
}