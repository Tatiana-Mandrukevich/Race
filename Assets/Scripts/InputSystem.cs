using UnityEngine;
using Zenject;

public class InputSystem : ITickable
{
    public bool IsLeftArrowButtonClicked;
    public bool IsRightArrowButtonClicked;
    public bool IsUpArrowButtonClicked;
    public bool IsDownArrowButtonClicked;

    private void Update()
    {
        IsLeftArrowButtonClicked = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        IsRightArrowButtonClicked = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        IsUpArrowButtonClicked = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        IsDownArrowButtonClicked = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    public void Tick()
    {
        Update();
    }
}