using DG.Tweening;
using UnityEngine;

public class FlyBuff : IBuff
{
    private IMultiplierSpeedForBuff _multiplierSpeedForBuff;
    private Transform _car;
    private CoinFlySpawner _coinFlySpawner;
    private Transform _camera;
    private Rigidbody _carRb;
    private ChunkMover _chunkMover;
    private bool _initialKinematic;
    private float _currentTime;
    public int Duration => 10;

    private const float SpeedMultiplier = 0.3f;
    private const float StopSpawnBeforeEnd = 1f;

    public FlyBuff(IMultiplierSpeedForBuff multiplierSpeedForBuff, Transform car, CoinFlySpawner coinFlySpawner, Transform camera)
    {
        _multiplierSpeedForBuff = multiplierSpeedForBuff;
        _car = car;
        _coinFlySpawner = coinFlySpawner;
        _camera = camera;
        _currentTime = 0;
        
        var carComponent = _car.GetComponent<Car>();
        if (carComponent != null)
        {
            _chunkMover = carComponent.ChunkMover;
        }
    }

    public void StartBuff()
    {
        _carRb = _car.GetComponent<Rigidbody>();
        if (_carRb != null)
        {
            _initialKinematic = _carRb.isKinematic;
            _carRb.isKinematic = true;
        }

        _camera.transform.DOLocalMoveY(_camera.transform.localPosition.y + 10, 1f).SetId("FlyBuffTween");
        _car.transform.DOLocalMoveY(_car.transform.localPosition.y + 10, 1f).SetId("FlyBuffTween");
        _multiplierSpeedForBuff.AddSpeedMultiplier(SpeedMultiplier);

        var cameraFollow = _camera.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.enabled = false;
        }
    }

    public void EndBuff()
    {
        DOTween.Kill("FlyBuffTween");
        _camera.transform.DOLocalMoveY(_camera.transform.localPosition.y - 10, 1f).OnComplete(() =>
        {
            var cameraFollow = _camera.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.enabled = true;
            }
        });
        _car.transform.DOLocalMoveY(_car.transform.localPosition.y - 10, 1f).OnComplete(() =>
        {
            if (_carRb != null)
            {
                _carRb.isKinematic = _initialKinematic;
            }
        });
        _multiplierSpeedForBuff.RemoveSpeedMultiplier(SpeedMultiplier);
    }

    public void Tick()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime < Duration - StopSpawnBeforeEnd)
        {
            _coinFlySpawner.SpawnCoin(_car.position.y);
        }
    }
}