using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkManager : MonoBehaviour
{
    public Transform CameraTransform;
    public List<GameObject> Chunks = new List<GameObject>();

    public int InitialBlockCount = 16;
    public float BlockLenght = 10;

    public float StartMoveSpeed = 10;
    public float MaxSpeed = 30f;
    public float SpeedIncreasePerSecond = 0.4f;

    public float recycleDistanceBehindCamera = 15;

    // Input System параметры
    public float LateralMoveSpeed = 5f; // Скорость плавного движения по X
    public float LateralInputSpeed = 2f; // Скорость изменения целевой позиции при зажатии клавиши

    private List<GameObject> _lastChunks = new List<GameObject>();
    private List<Transform> _activeChunks = new List<Transform>();
    private float _currentSpeed = 0;
    
    // Переменные для движения по X
    private float _targetLateralPosition = 0f; // Целевая позиция по X (-1, 0, 1)
    private float _currentLateralPosition = 0f; // Текущая позиция по X

    private void Awake()
    {
        SpawnInitialChunks();
    }

    private void Update()
    {
        RecalculateSpeed();
        MoveBlocks(_currentSpeed);
        RecycleBlockPassedCamera();
        
        HandleLateralInput();
        UpdateLateralPosition();
    }

    private void RecalculateSpeed()
    {
        if (_currentSpeed < StartMoveSpeed)
        {
            _currentSpeed += StartMoveSpeed * Time.deltaTime;
            if (_currentSpeed > StartMoveSpeed)
            {
                _currentSpeed = StartMoveSpeed;
            }
        }
        else
        {
            if (_currentSpeed != MaxSpeed)
            {
                _currentSpeed += SpeedIncreasePerSecond * Time.deltaTime;
            }
        }

        if (_currentSpeed > MaxSpeed)
        {
            _currentSpeed = MaxSpeed;
        }
    }

    private void SpawnInitialChunks()
    {
        float nextSpawnPositionZ = CameraTransform.position.z;
        for (int i = 0; i < InitialBlockCount; i++)
        {
            Transform spawnedChunk = InstantiateChunk(nextSpawnPositionZ);
            _activeChunks.Add(spawnedChunk);
            nextSpawnPositionZ += BlockLenght;
        }
    }

    private GameObject GetChunk()
    {
        if (_lastChunks.Count >= 2)
        {
            var randomChunk = Chunks[Random.Range(0, Chunks.Count)];
            while (_lastChunks.Contains(randomChunk))
            {
                randomChunk = Chunks[Random.Range(0, Chunks.Count)];
            }
            return randomChunk;
        }
        return Chunks[Random.Range(0, Chunks.Count)];
    }

    private void MoveBlocks(float moveSpeed)
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 moveOffset = new Vector3(0, 0, -moveDistance);
        foreach (var activeChunk in _activeChunks)
        {
            activeChunk.transform.position += moveOffset;
        }
    }

    private void RecycleBlockPassedCamera()
    {
        float recycleThreshold = CameraTransform.position.z - recycleDistanceBehindCamera;
        while (_activeChunks.Count > 0)
        {
            Transform oldestBlock = _activeChunks[0];
            if (oldestBlock.position.z >= recycleThreshold)
            {
                return;
            }
            Transform recycleChunk = _activeChunks[0];
            _activeChunks.Remove(recycleChunk);
            float frontBlockZPosition = _activeChunks.Count == 0 ? recycleChunk.position.z : GetFrontPositionZ();
            float nextBlockZPosition = frontBlockZPosition + BlockLenght;
            Destroy(oldestBlock.gameObject);
            Transform newBlock = InstantiateChunk(nextBlockZPosition);
            _activeChunks.Add(newBlock);
        }
    }

    private float GetFrontPositionZ()
    {
        float returnValue = float.MinValue;
        foreach (var activeChunk in _activeChunks)
        {
            if (activeChunk.position.z > returnValue)
            {
                returnValue = activeChunk.position.z;
            }
        }
        return returnValue;
    }

    private Transform InstantiateChunk(float zPosition)
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
        GameObject lastChunk = GetChunk();
        GameObject newChunk = Instantiate(lastChunk);
        if (_lastChunks.Count >= 2)
        {
            _lastChunks.Remove(_lastChunks.First());
        }
        _lastChunks.Add(lastChunk);
        newChunk.transform.position = spawnPosition;
        newChunk.transform.SetParent(transform);
        return newChunk.transform;
    }

    private void HandleLateralInput()
    {
        // Проверка зажатия клавиш A или левой стрелки (инвертировано - едет вправо)
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _targetLateralPosition += LateralInputSpeed * Time.deltaTime;
        }
        
        // Проверка зажатия клавиш D или правой стрелки (инвертировано - едет влево)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _targetLateralPosition -= LateralInputSpeed * Time.deltaTime;
        }
        
        // Ограничиваем целевую позицию диапазоном [-1, 1]
        _targetLateralPosition = Mathf.Clamp(_targetLateralPosition, -1f, 1f);
    }

    private void UpdateLateralPosition()
    {
        // Плавная интерполяция текущей позиции к целевой
        _currentLateralPosition = Mathf.Lerp(
            _currentLateralPosition,
            _targetLateralPosition,
            LateralMoveSpeed * Time.deltaTime
        );

        // Обновляем позицию ChunkManager по X
        Vector3 newPosition = transform.position;
        newPosition.x = _currentLateralPosition;
        transform.position = newPosition;
    }
}