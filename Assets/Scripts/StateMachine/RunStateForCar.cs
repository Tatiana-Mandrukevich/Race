using DG.Tweening;
using UnityEngine;

public class RunStateForCar : State
{
    private WheelCarModule _wheelModule; 
    private ISpeedManager _speedManager;
    private CarRotateModule _carRotateModule;
    private Transform _car;
        
    public RunStateForCar(WheelCarModule wheelModule, ISpeedManager speedManager, CarRotateModule carRotateModule, Transform car)
    {
        _wheelModule = wheelModule;
        _speedManager = speedManager;
        _carRotateModule = carRotateModule;
        _car = car;
    }

    public override void Tick()
    {
        _carRotateModule.Tick();
    }

    public override void OnStateEnter()
    {
        _car.transform.DOLocalRotate(new Vector3(-6, 0, 0), 0.4f).OnComplete(() =>
        {
            _car.transform.DOLocalRotate(Vector3.zero, 0.2f);
        });
        _wheelModule.StartWheel();
        _speedManager.SetIsMove(true);
    }

    public override void OnStateExit()
    {
        _wheelModule.StopWheel();
        _speedManager.SetIsMove(false);
    }
}