using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    public Vector2 move;
    public bool jump;
    public bool sprint;
    public Vector2 point;

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }

    public void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    public void OnPoint(InputValue value)
    {
        point = value.Get<Vector2>();
    }
}
