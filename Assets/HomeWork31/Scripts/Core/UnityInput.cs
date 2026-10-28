using UnityEngine;

public class UnityInput : IInput
{
    private const int LeftMouseButton = 0;
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    public Vector3 GetMoveDirection()
    {
        float horizontal = Input.GetAxisRaw(HorizontalAxis);
        float vertical = Input.GetAxisRaw(VerticalAxis);
        return new Vector3(horizontal, 0, vertical);
    }

    public bool IsShootPressed()
    {
        return Input.GetMouseButtonDown(LeftMouseButton) || Input.GetKeyDown(KeyCode.Space);
    }
}