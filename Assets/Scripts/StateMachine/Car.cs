using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform[] wheels;
    public ChunkManager _chunkManager;
    
    private StateMachine _stateMachine;
    private WheelCarModule _wheelModule;

    private void Start()
    {
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _wheelModule = new WheelCarModule(_chunkManager, wheels);
        
        State idleState = new IdleStateForCar(transform);
        State runState = new RunStateForCar(_wheelModule, _chunkManager.SpeedManager);
        
        idleState.AddTransition(new StateTransition(runState, new FuncCondition(() => _chunkManager.IsMove)));
        runState.AddTransition(new StateTransition(idleState, new FuncCondition(() => !_chunkManager.IsMove)));
        _stateMachine = new StateMachine(idleState);
    }

    private void Update()
    {
        _stateMachine.Tick();
        _wheelModule.Tick();
    }
}