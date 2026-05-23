using System;
using UnityEngine;
using Zenject;

public class Car : MonoBehaviour
{
    public Transform[] wheels;
    public Transform[] FrontWheels;
    public ChunkManager _chunkManager;
    
    private StateMachine _stateMachine;
    private WheelCarModule _wheelModule;
    private CarRotateModule _carRotateModule;
    private CarCrushState _crushState;
    private ChunkMover _chunkMover;
    private WheelRotateModule _wheelRotateModule;
    
    [Inject] private InputSystem _inputSystem;

    private void Start()
    {
        InitializeStateMachine();
    }
    
    public void Crush()
    {
        _stateMachine.SetState(_crushState);
    }

    private void InitializeStateMachine()
    {
        _chunkMover = new ChunkMover(_chunkManager.transform, _inputSystem);
        _wheelModule = new WheelCarModule(_chunkManager, wheels);
        _carRotateModule = new CarRotateModule(_inputSystem, transform, _chunkMover);
        _wheelRotateModule = new WheelRotateModule(_carRotateModule, FrontWheels);
        State idleState = new IdleStateForCar(transform);
        State runState = new RunStateForCar(_wheelModule, _chunkManager.SpeedManager, _carRotateModule, transform);
        
        idleState.AddTransition(new StateTransition(runState, new FuncCondition(() => _chunkManager.IsMove)));
        runState.AddTransition(new StateTransition(idleState, new FuncCondition(() => !_chunkManager.IsMove)));

        _crushState = new CarCrushState(_wheelModule, _carRotateModule, _chunkManager.SpeedManager, transform);
        _crushState.AddTransition(new StateTransition(runState, new FuncCondition(() => _crushState.IsFinish && _chunkManager.IsMove)));
        _crushState.AddTransition(new StateTransition(idleState, new FuncCondition(() => _crushState.IsFinish && !_chunkManager.IsMove)));

        _stateMachine = new StateMachine(idleState);
    }

    private void Update()
    {
        _chunkMover.HandleLateralInput();
        _stateMachine.Tick();
        _wheelModule.Tick();
        _chunkMover.UpdateLateralPosition();
        _wheelRotateModule.Tick();
    }
}