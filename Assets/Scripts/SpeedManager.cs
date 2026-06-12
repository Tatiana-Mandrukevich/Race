using System;
using UnityEngine;

public class SpeedManager : ISpeedManager, ISpeedForBuff, IMultiplierSpeedForBuff
{
    private float _currentSpeed;
    private readonly float _startSpeed;
    private readonly float _maxSpeed;
    private readonly float _increaseRate;
    private bool _isMove;
    private float speedMultiplier;

    public static event Action<bool> OnLost;
    private bool _isLost;
    public bool IsLost => _isLost;

    public SpeedManager(float startSpeed, float maxSpeed, float increaseRate, float speedMultiplier)
    {
        _startSpeed = startSpeed;
        _maxSpeed = maxSpeed;
        _increaseRate = increaseRate;
        this.speedMultiplier = speedMultiplier;
    }

    public float GetCurrentSpeed()
    {
        if (!_isMove)
        {
            _currentSpeed = 0;
            return 0;
        }
        
        if (_currentSpeed < _startSpeed)
        {
            _currentSpeed += _startSpeed * Time.deltaTime;
            if (_currentSpeed > _startSpeed)
            {
                _currentSpeed = _startSpeed;
            }
        }
        else
        {
            if (_currentSpeed != _maxSpeed)
            {
                _currentSpeed += _increaseRate * Time.deltaTime;
            }
        }

        if (_currentSpeed > _maxSpeed)
        {
            _currentSpeed = _maxSpeed;
        }

        return _currentSpeed;
    }

    public void SetIsMove(bool isMove)
    {
        _isMove = isMove;
    }
    
    public void ReduceSpeedAfterCrush()
    {
        _currentSpeed *= 0.7f;
        if (_currentSpeed < _startSpeed)
        {
            Debug.Log("Lose");
            _isLost = true;
            OnLost?.Invoke(true);
        }
        else
        {
            OnLost?.Invoke(false);
        }
    }

    public void ResetIsLost()
    {
        _isLost = false;
        _currentSpeed = _startSpeed;
        OnLost?.Invoke(false);
    }
    
    public void AddSpeed(float speed)
    {
        _currentSpeed += speed;
    }

    public void RemoveSpeed(float speed)
    {
        _currentSpeed -= speed;
        if (_currentSpeed < _startSpeed) _currentSpeed = _startSpeed;
    }

    public void AddSpeedMultiplier(float speedMultiplier)
    {
        this.speedMultiplier += speedMultiplier;
    }

    public void RemoveSpeedMultiplier(float speedMultiplier)
    {
        this.speedMultiplier -= speedMultiplier;
    }
}
