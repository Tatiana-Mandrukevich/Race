using UnityEngine;

public class WheelRotateModule
{
    private CarRotateModule _carRotateModule;
    private Transform[]  _frontWheels;
    
    private const float Angle = 60f;
    private const float Speed = 8f;
    
    public WheelRotateModule(CarRotateModule carRotateModule, Transform[] frontWheels)
    {
        _carRotateModule = carRotateModule;
        _frontWheels = frontWheels;
    }
    
    public void Tick()
    {
        HandleRotationInput();
    }

    private void HandleRotationInput()
    {
        if (_carRotateModule.TargetRotationAngle > 0f)
        {
            foreach (var wheel in _frontWheels)
            {
                Quaternion targetRotation = Quaternion.Euler(0, Angle, 0);
                wheel.transform.localRotation = Quaternion.Lerp(wheel.transform.localRotation, targetRotation, Speed * Time.deltaTime);
            }
        }
        else if (_carRotateModule.TargetRotationAngle < 0f)
        {
            foreach (var wheel in _frontWheels)
            {
                Quaternion targetRotation = Quaternion.Euler(0, -Angle, 0);
                wheel.transform.localRotation = Quaternion.Lerp(wheel.transform.localRotation, targetRotation, Speed * Time.deltaTime);
            }
        }
        else
        {
            foreach (var wheel in _frontWheels)
            {
                Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
                wheel.transform.localRotation = Quaternion.Lerp(wheel.transform.localRotation, targetRotation, Speed * Time.deltaTime);
            }
        }
    }
}