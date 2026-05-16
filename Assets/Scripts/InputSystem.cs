using UnityEngine;
using Zenject;

public class InputSystem : ITickable
{
    public bool IsLeftArrowButtonClicked;
    public bool IsRightArrowButtonClicked;

    private void Update()
    {
        IsLeftArrowButtonClicked = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        IsRightArrowButtonClicked = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
    }

    public void Tick()
    {
        Update();
    }
}