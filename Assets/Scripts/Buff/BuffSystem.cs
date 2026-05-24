using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    public CarTrigger CarTrigger;
    public SpeedManager SpeedManager;
    public CoinFlySpawner FlySpawner;
    public bool IsFlying { get; private set; }

    private List<IBuff> _buffs = new List<IBuff>();

    private void Update()
    {
        foreach (var buff in _buffs)
        {
            buff.Tick();
        }
    }

    public void AddBuff(IBuff buff)
    {
        buff.StartBuff();
        _buffs.Add(buff);
        DOVirtual.DelayedCall(buff.Duration, () => { RemoveBuff(buff); });
    }

    public void RemoveBuff(IBuff buff)
    {
        if (buff is FlyBuff)
        {
            IsFlying = false;
        }
        buff.EndBuff();
        _buffs.Remove(buff);
    }

    public void AddFlyBuff()
    {
        if (CarTrigger == null)
        {
            CarTrigger = FindObjectOfType<CarTrigger>();
        }

        if (SpeedManager == null)
        {
            SpeedManager = FindObjectOfType<ChunkManager>()?.SpeedManager as SpeedManager;
        }

        if (FlySpawner == null)
        {
            FlySpawner = FindObjectOfType<CoinFlySpawner>();
        }
        
        IsFlying = true;
        if (CarTrigger != null && CarTrigger.Car != null)
        {
            AddBuff(new FlyBuff(SpeedManager, CarTrigger.Car.transform, FlySpawner, Camera.main.transform));
        }
        else
        {
            Car car = FindObjectOfType<Car>();
            if (car != null)
            {
                AddBuff(new FlyBuff(SpeedManager, car.transform, FlySpawner, Camera.main.transform));
            }
            else
            {
                Debug.LogError("Car not found for FlyBuff!");
            }
        }
    }

    public void AddSpeedBuff()
    {
        
    }
}