using main.entity.Turn_System;
using main.service.Turn_System;
using Zenject;

namespace main.infrastructure
{
    public class EffectAssemblyServiceInstaller : MonoInstaller<EffectAssemblyServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<EffectAssembly>().AsSingle();
            Container.BindInterfacesAndSelfTo<EffectAssemblyService>().AsSingle();
        }
    }
}