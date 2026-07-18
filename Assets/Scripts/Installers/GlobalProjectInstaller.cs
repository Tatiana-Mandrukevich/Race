using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalProjectInstaller : MonoInstaller
    {
        [SerializeField] private AudioManager audioManagerPrefab;

        public override void InstallBindings()
        {
            // Спавним префаб аудиоменеджера в глобальный контекст. 
            // Он автоматически получит DontDestroyOnLoad и будет доступен на ВСЕХ сценах.
            Container.Bind<AudioManager>()
                .FromComponentInNewPrefab(audioManagerPrefab)
                .AsSingle()
                .NonLazy(); // NonLazy заставит музыку включиться СРАЗУ при старте игры
        }
    }
}