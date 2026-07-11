using System;
using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    
    public void DoSmall(Action completeAction)
    {
        transform.DOScale(transform.localScale * 0.5f,  0.05f).SetEase(Ease.InOutSine)
            .OnComplete(completeAction.Invoke);
    }
}
