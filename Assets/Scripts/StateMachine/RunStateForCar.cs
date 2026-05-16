public class RunStateForCar : State
{
    private WheelCarModule _wheelModule; 
    private ISpeedManager _speedManager;
        
    public RunStateForCar(WheelCarModule wheelModule, ISpeedManager speedManager)
    {
        _wheelModule = wheelModule;
        _speedManager = speedManager;
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