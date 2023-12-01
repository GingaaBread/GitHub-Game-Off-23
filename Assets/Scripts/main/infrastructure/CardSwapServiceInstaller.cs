using main.service.Turn_System;
using Zenject;

namespace main.infrastructure
{
    public class CardSwapServiceInstaller : MonoInstaller<CardSwapServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CardSwapService>().AsSingle();
        }
    }
}