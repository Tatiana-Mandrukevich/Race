using Zenject;
using UnityEngine;

public class CarTrigger : MonoBehaviour, IInvisibleForBuff
{
    private ISpeedManager _speedManager;
    public Car Car;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            Car.Crush();
            other.GetComponent<CarCrashAnimation>()?.OnCrash(transform);

            if (_speedManager == null)
            {
                _speedManager = Car._chunkManager.SpeedManager;
            }

            _speedManager.ReduceSpeedAfterCrush();
        }
    }

    public void SetInvisible(bool invisible)
    {
        GetComponent<Collider>().enabled = !invisible;
    }
}