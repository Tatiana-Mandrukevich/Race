using System;
using UnityEngine;
using Zenject;

public class Car : MonoBehaviour
{
    public Transform[] wheels;
    public ChunkManager _chunkManager;
    
    private StateMachine _stateMachine;
    private WheelCarModule _wheelModule;
    private CarRotateModule _carRotateModule;
    private ChunkMover _chunkMover;
    
    [Inject] private InputSystem _inputSystem;

    private void Start()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _chunkMover = new ChunkMover(_chunkManager.transform, _inputSystem);
        _wheelModule = new WheelCarModule(_chunkManager, wheels);
        _carRotateModule = new CarRotateModule(_inputSystem, transform, _chunkMover);
        State idleState = new IdleStateForCar(transform);
        State runState = new RunStateForCar(_wheelModule, _chunkManager.SpeedManager, _carRotateModule);
        
        idleState.AddTransition(new StateTransition(runState, new FuncCondition(() => _chunkManager.IsMove)));
        runState.AddTransition(new StateTransition(idleState, new FuncCondition(() => !_chunkManager.IsMove)));
        _stateMachine = new StateMachine(idleState);
    }

    private void Update()
    {
        _chunkMover.HandleLateralInput();
        _stateMachine.Tick();
        _wheelModule.Tick();
        _chunkMover.UpdateLateralPosition();
    }
}