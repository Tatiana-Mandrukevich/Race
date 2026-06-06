using System.Collections.Generic;
using UnityEngine;

public class ChunkMover : IChunkMover
{
    private float _minPosition = -1f;
    private float _maxPosition = 1f;
    
    private InputSystem _inputSystem;
    private Transform _chunkManagerTransform;
    
    // Input System параметры
    public float LateralMoveSpeed = 5f; // Скорость плавного движения по X
    public float LateralInputSpeed = 2f; // Скорость изменения целевой позиции при зажатии клавиши
    
    // Переменные для движения по X
    private float _targetLateralPosition = 0f; // Целевая позиция по X (-1, 0, 1)
    private float _currentLateralPosition = 0f; // Текущая позиция по X
    
    public float CurrentLateralPosition => _currentLateralPosition;
    public float MinPosition => _minPosition;
    public float MaxPosition => _maxPosition;
    
    public ChunkMover(Transform chunkManagerTransform, InputSystem inputSystem)
    {
        _chunkManagerTransform = chunkManagerTransform;
        _inputSystem = inputSystem;
    }
    
    public void MoveForward(List<Transform> chunks, float speed)
    {
        float moveDistance = speed * Time.deltaTime;
        Vector3 moveOffset = new Vector3(0, 0, -moveDistance);
        for (int i = 0; i < chunks.Count; i++)
        {
            chunks[i].position += moveOffset;
        }
    }
    
    public void HandleLateralInput()
    {
        // Проверка зажатия клавиш A или левой стрелки (инвертировано - едет вправо)
        if (_inputSystem.IsLeftArrowButtonClicked)
        {
            _targetLateralPosition += LateralInputSpeed * Time.deltaTime;
        }
        
        // Проверка зажатия клавиш D или правой стрелки (инвертировано - едет влево)
        if (_inputSystem.IsRightArrowButtonClicked)
        {
            _targetLateralPosition -= LateralInputSpeed * Time.deltaTime;
        }
        
        // Ограничиваем целевую позицию диапазоном [-1, 1]
        _targetLateralPosition = Mathf.Clamp(_targetLateralPosition, _minPosition, _maxPosition);
    }

    public void SetLateralLimits(float min, float max)
    {
        _minPosition = min;
        _maxPosition = max;
    }

    public void UpdateLateralPosition()
    {
        // Плавная интерполяция текущей позиции к целевой
        _currentLateralPosition = Mathf.Lerp(
            _currentLateralPosition,
            _targetLateralPosition,
            LateralMoveSpeed * Time.deltaTime
        );

        // Обновляем позицию ChunkManager по X
        Vector3 newPosition = _chunkManagerTransform.position;
        newPosition.x = _currentLateralPosition;
        _chunkManagerTransform.position = newPosition;
    }
}