using UnityEngine;

// For getting the mousePositionDelta, the built-in Input class does not have mousePositionDelta for some reason
public class Mouse
{
    public Vector3 mousePosition;
    public Vector3 mousePositionDelta;

    public void Update()
    {
        mousePositionDelta = Input.mousePosition - mousePosition;
        mousePosition = Input.mousePosition;
    }
}
