using DefaultNamespace.Buff;
using Zenject;

namespace Installers
{
    public class RaceProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputSystem>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<CoinController>().FromNew().AsSingle();
        }
    }
}