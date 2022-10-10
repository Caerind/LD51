using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : Singleton<Inputs>
{
    public Vector2 move;
    public Vector2 look;
    public bool fire;
    public bool cac;
    public bool tuto;
    public bool nextLeft;
    public bool nextRight;
    public Vector2 point;

    private bool usingGamepad;

    public bool IsUsingGamepad()
    {
        return usingGamepad;
    }

    public bool IsUsingKeyboardMouse()
    {
        return !usingGamepad;
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
        usingGamepad = true;
        MouseCursor.Instance.ShowCursor(false);
    }

    public void OnFire(InputValue value)
    {
        fire = value.isPressed;
    }

    public void OnCac(InputValue value)
    {
        cac = value.isPressed;
    }

    public void OnTuto(InputValue value)
    {
        tuto = value.isPressed;
    }

    public void OnNextLeft(InputValue value)
    {
        nextLeft = value.isPressed;
    }

    public void OnNextRight(InputValue value)
    {
        nextRight = value.isPressed;
    }

    public void OnPoint(InputValue value)
    {
        point = value.Get<Vector2>();
        usingGamepad = false;
        MouseCursor.Instance.ShowCursor(true);
    }
}
