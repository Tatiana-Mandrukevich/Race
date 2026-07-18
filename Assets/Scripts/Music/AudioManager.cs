using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Аудиофайлы (Audio Clips)")]
    [SerializeField] private AudioClip backgroundMusic; // Музыка главного меню и игры
    [SerializeField] private AudioClip coinCollectSound;
    [SerializeField] private AudioClip carFlySound;
    [SerializeField] private AudioClip carIdleSound;
    [SerializeField] private AudioClip carCrashSound;

    [Header("Источники звука (Audio Sources)")]
    [SerializeField] private AudioSource sfxSource;      
    [SerializeField] private AudioSource carLoopSource;  
    [SerializeField] private AudioSource musicSource;    // Отдельный источник для фоновой музыки

    private void Start()
    {
        // Как только игра запускается (на первой сцене), сразу включаем фоновую музыку
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource.isPlaying) return;
        
        musicSource.clip = backgroundMusic;
        musicSource.loop = true; // Музыка должна быть зациклена
        musicSource.Play();
    }

    public void PlayCoinCollect() => sfxSource.PlayOneShot(coinCollectSound);
    public void PlayCarFly() => sfxSource.PlayOneShot(carFlySound);
    
    public void PlayCarCrash()
    {
        StopCarLoop();
        sfxSource.PlayOneShot(carCrashSound);
    }

    public void PlayCarIdle()
    {
        if (carLoopSource.isPlaying && carLoopSource.clip == carIdleSound) return;
        carLoopSource.clip = carIdleSound;
        carLoopSource.loop = true; 
        carLoopSource.Play();
    }

    public void StopCarLoop() => carLoopSource.Stop();
}