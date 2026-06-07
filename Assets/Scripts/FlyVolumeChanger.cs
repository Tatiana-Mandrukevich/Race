using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FlyVolumeChanger : MonoBehaviour
{
    public Volume volume;
    private LensDistortion _lensDistortion;
    
    public static FlyVolumeChanger Instance;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void StartFlyEffect(float duration)
    {
        if (volume.profile.TryGet(out _lensDistortion))
        {
            DOTween.To(() => _lensDistortion.intensity.value, x => _lensDistortion.intensity.value = x, -1f, duration);
            DOTween.To(() => _lensDistortion.scale.value, x => _lensDistortion.scale.value = x, 3f, duration);
        }
    }

    public void StopFlyEffect(float duration)
    {
        if (volume.profile.TryGet(out _lensDistortion))
        {
            DOTween.To(() => _lensDistortion.intensity.value, x => _lensDistortion.intensity.value = x, 0f, duration);
            DOTween.To(() => _lensDistortion.scale.value, x => _lensDistortion.scale.value = x, 1f, duration);
        }
    }
}
