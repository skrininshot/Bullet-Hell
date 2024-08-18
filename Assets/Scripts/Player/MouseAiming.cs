using UnityEngine;
using UnityEngine.Events;

public class MouseAiming : MouseRotate
{
    [HideInInspector] public UnityEvent OnClick = new();

    protected override void Update()
    {
        base.Update();
        HandleClick();
    }

    private void HandleClick()
    {
        bool mouseClicked = Input.GetMouseButtonDown(0);

        if (mouseClicked) { OnClick?.Invoke(); }
    }
}