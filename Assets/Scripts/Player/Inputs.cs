using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : Singleton<Inputs>
{
    public Vector2 move;
    public Vector2 look;
    public bool fire;
    public bool tuto;
    public bool nextLeft;
    public bool nextRight;
    public Vector2 point;

    private PlayerInput input;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
    }

    public PlayerInput GetInput()
    {
        return input;
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        fire = value.isPressed;
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
    }
}
