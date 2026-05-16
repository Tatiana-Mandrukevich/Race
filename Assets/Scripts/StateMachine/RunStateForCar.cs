public class RunStateForCar : State
{
    private WheelCarModule _wheelModule; 
    private ISpeedManager _speedManager;
    private CarRotateModule _carRotateModule;
        
    public RunStateForCar(WheelCarModule wheelModule, ISpeedManager speedManager, CarRotateModule carRotateModule)
    {
        _wheelModule = wheelModule;
        _speedManager = speedManager;
        _carRotateModule = carRotateModule;
    }

    public override void Tick()
    {
        _carRotateModule.Tick();
    }

    public override void OnStateEnter()
    {
        _wheelModule.StartWheel();
        _speedManager.SetIsMove(true);
    }

    public override void OnStateExit()
    {
        _wheelModule.StopWheel();
        _speedManager.SetIsMove(false);
    }
}