using UnityEngine;

public class SpeedManager : ISpeedManager
{
    private float _currentSpeed;
    private readonly float _startSpeed;
    private readonly float _maxSpeed;
    private readonly float _increaseRate;

    public SpeedManager(float startSpeed, float maxSpeed, float increaseRate)
    {
        _startSpeed = startSpeed;
        _maxSpeed = maxSpeed;
        _increaseRate = increaseRate;
        _currentSpeed = 0f;
    }

    public float GetCurrentSpeed()
    {
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
}
