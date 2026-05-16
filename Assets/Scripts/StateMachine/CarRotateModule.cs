using UnityEngine;

public class CarRotateModule
{
    private InputSystem _inputSystem;
    private Transform _car;
    private ChunkMover _chunkMover;

    private const float Angle = 15f;
    private const float Speed = 8f;
    
    private float _targetRotationAngle = 0f;
    
    public float TargetRotationAngle => _targetRotationAngle;
        
    public CarRotateModule(InputSystem inputSystem, Transform car, ChunkMover chunkMover)
    {
        _inputSystem = inputSystem;
        _car = car;
        _chunkMover = chunkMover;
    }

    public void Tick()
    {
        HandleRotationInput();
        
        Quaternion targetRotation = Quaternion.Euler(0, _targetRotationAngle, 0);
        _car.transform.localRotation = Quaternion.Lerp(_car.transform.localRotation, targetRotation, Speed * Time.deltaTime);
    }
    
    private void HandleRotationInput()
    {
        bool isLeftPulled = IsCarPulledToLeft();
        bool isRightPulled = IsCarPulledToRight();
        bool isLeftPressed = _inputSystem.IsLeftArrowButtonClicked;
        bool isRightPressed = _inputSystem.IsRightArrowButtonClicked;
        
        // Левая стрелка движет машину вправо, упирается в правую границу
        if (isRightPulled && isLeftPressed)
        {
            _targetRotationAngle = 0f;
        }
        // Правая стрелка движет машину влево, упирается в левую границу
        else if (isLeftPulled && isRightPressed)
        {
            _targetRotationAngle = 0f;
        }
        // При нажатии левой стрелки машина поворачивается влево
        else if (isLeftPressed)
        {
            _targetRotationAngle = -Angle;
        }
        // При нажатии правой стрелки машина поворачивается вправо
        else if (isRightPressed)
        {
            _targetRotationAngle = Angle;
        }
        // Если ничего не нажато, машина поворачивается прямо
        else
        {
            _targetRotationAngle = 0f;
        }
    }

    // Проверяем, находится ли текущая позиция на минимальной границе (с небольшой погрешностью)
    private bool IsCarPulledToLeft()
    {
        return _chunkMover.CurrentLateralPosition <= _chunkMover.MinPosition + 0.1f;
    }

    // Проверяем, находится ли текущая позиция на максимальной границе (с небольшой погрешностью)
    private bool IsCarPulledToRight()
    {
        return _chunkMover.CurrentLateralPosition >= _chunkMover.MaxPosition - 0.1f;
    }
}