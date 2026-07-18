using Zenject;
using UnityEngine;

public class CarTrigger : MonoBehaviour, IInvisibleForBuff
{
    private ISpeedManager _speedManager;
    public Car Car;
    
    [Inject] private AudioManager _audioManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            CrushVolumeChanger.Instance.Crush();
            Car.Crush();
            other.GetComponent<CarCrashAnimation>()?.OnCrash(transform);

            if (_speedManager == null)
            {
                _speedManager = Car._chunkManager.SpeedManager;
            }

            _speedManager.ReduceSpeedAfterCrush();
            
            _audioManager.PlayCarCrash();
        }
    }

    public void SetInvisible(bool invisible)
    {
        GetComponent<Collider>().enabled = !invisible;
    }
}