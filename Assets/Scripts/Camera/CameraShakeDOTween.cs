using UnityEngine;
using DG.Tweening;

public class CameraShakeDOTween : MonoBehaviour
{
    public static CameraShakeDOTween Instance { get; private set; }

    private Transform _camTransform;
    private Vector3 _initialLocalPosition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _camTransform = Camera.main != null ? Camera.main.transform : transform;
        _initialLocalPosition = _camTransform.localPosition;
    }

    public void Shake(float duration = 0.2f, float strength = 0.25f, int vibrato = 10, float randomness = 90f)
    {
        _camTransform.DOKill();
        _camTransform.localPosition = _initialLocalPosition;
        
        _camTransform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }
}

