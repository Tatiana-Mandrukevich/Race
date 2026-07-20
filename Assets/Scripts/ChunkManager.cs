using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ChunkManager : MonoBehaviour
{
    public List<GameObject> Chunks = new();
    
    [SerializeField] private Transform CameraTransform;
    [SerializeField] private int InitialBlockCount = 16;
    [SerializeField] private float BlockLenght = 10;
    [SerializeField] private float StartMoveSpeed = 10;
    [SerializeField] private float MaxSpeed = 30f;
    [SerializeField] private float SpeedIncreasePerSecond = 0.4f;
    [SerializeField] private float recycleDistanceBehindCamera = 15;

    private float speedMultiplier;
    private List<GameObject> _lastChunks = new();
    private List<Transform> _activeChunks = new();
    
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
        _speedManager.SetIsMove(IsMove);
        float speed = _speedManager.GetCurrentSpeed();

        if (!_speedManager.IsLost)
        {
            _mover.MoveForward(_activeChunks, speed);
            _mover.HandleLateralInput();
        }
        
        _recycler.RecycleChunks(_activeChunks, CameraTransform.position.z - recycleDistanceBehindCamera, BlockLenght, _spawner);
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