using UnityEngine;

public class SpeedManager : ISpeedManager
{
    private float _currentSpeed;
    private readonly float _startSpeed;
    private readonly float _maxSpeed;
    private readonly float _increaseRate;
    private bool _isMove;

    public SpeedManager(float startSpeed, float maxSpeed, float increaseRate)
    {
        _startSpeed = startSpeed;
        _maxSpeed = maxSpeed;
        _increaseRate = increaseRate;
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
}
