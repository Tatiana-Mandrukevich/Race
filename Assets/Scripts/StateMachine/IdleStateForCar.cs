using DG.Tweening;
using UnityEngine;

public class IdleStateForCar : State
{
    private Transform _car;
    private Sequence _carSequence;
    private Vector3 _startScale;
    private AudioManager _audioManager;
    
    public IdleStateForCar(Transform car, AudioManager audioManager)
    {
        _car = car;
        _startScale = car.localScale;
        _audioManager = audioManager;
    }

    public override void OnStateEnter()
    {
        _carSequence = DOTween.Sequence();
        _carSequence.Append(_car.DOShakeScale(0.2f, 0.01f));
        _carSequence.SetLoops(-1, LoopType.Yoyo);
        _audioManager.PlayCarIdle();
    }

    public override void OnStateExit()
    {
        _carSequence.Kill();
        _car.transform.localScale = _startScale;
        _audioManager.StopCarLoop();
    }
}