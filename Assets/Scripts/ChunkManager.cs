using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ChunkManager : MonoBehaviour
{
    public Transform CameraTransform;
    public List<GameObject> Chunks = new List<GameObject>();

    public int InitialBlockCount = 16;
    public float BlockLenght = 10;

    public float StartMoveSpeed = 10;
    public float MaxSpeed = 30f;
    public float SpeedIncreasePerSecond = 0.4f;
    private float speedMultiplier;

    public float recycleDistanceBehindCamera = 15;

    private List<GameObject> _lastChunks = new List<GameObject>();
    private List<Transform> _activeChunks = new List<Transform>();
    public float CurrentSpeed => _speedManager.GetCurrentSpeed();
    public ISpeedManager SpeedManager => _speedManager;
    public bool IsMove => _inputSystem.IsUpArrowButtonClicked || IsFlying();

    private bool IsFlying()
    {
        var buffSystem = FindObjectOfType<BuffSystem>();
        return buffSystem != null && buffSystem.IsFlying;
    }
    
    private ISpeedManager _speedManager;
    private IChunkSpawner _spawner;
    private IChunkMover _mover;
    private IChunkRecycler _recycler;
    [Inject] private InputSystem _inputSystem;

    private void Awake()
    {
        // Создаем зависимости
        _speedManager = new SpeedManager(StartMoveSpeed, MaxSpeed, SpeedIncreasePerSecond, speedMultiplier);
        _spawner = new DefaultNamespace.ChunkSpawner(Chunks, _lastChunks, transform);
        _mover = new ChunkMover(transform, _inputSystem);
        _recycler = new ChunkRecycler();
        
        SetupCameraFollow();
        SpawnInitialChunks();
    }

    private void SetupCameraFollow()
    {
        if (CameraTransform != null && CameraTransform.GetComponent<CameraFollow>() == null)
        {
            CameraFollow follow = CameraTransform.gameObject.AddComponent<CameraFollow>();
            // Ищем машину в сцене
            Car car = FindObjectOfType<Car>();
            if (car != null)
            {
                follow.carTransform = car.transform;
                // Настраиваем смещение исходя из текущей разницы позиций, 
                // если машина и камера уже расставлены в сцене
                follow.offset = CameraTransform.position - car.transform.position;
                follow.fixedY = CameraTransform.position.y;
            }
        }
    }

    private void Update()
    {
        float speed = _speedManager.GetCurrentSpeed();
        _mover.MoveForward(_activeChunks, speed);
        _recycler.RecycleChunks(_activeChunks, CameraTransform.position.z - recycleDistanceBehindCamera, BlockLenght, _spawner);
        
        _mover.HandleLateralInput();
        _mover.UpdateLateralPosition();
    }

    private void SpawnInitialChunks()
    {
        float nextSpawnPositionZ = CameraTransform.position.z;
        for (int i = 0; i < InitialBlockCount; i++)
        {
            Transform spawnedChunk = _spawner.SpawnChunk(nextSpawnPositionZ);
            _activeChunks.Add(spawnedChunk);
            nextSpawnPositionZ += BlockLenght;
        }
    }
    
    private void MoveBlocks(float moveSpeed)
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        moveDistance += moveDistance * speedMultiplier;
        Vector3 moveOffset = new Vector3(0, 0, -moveDistance);
        foreach (var activeChunk in _activeChunks)
        {
            activeChunk.transform.position += moveOffset;
        }
    }
    
    public MonoPooled GetLastChunk()
    {
        return _activeChunks.Last().GetComponent<MonoPooled>();
    }
}