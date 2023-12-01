using main.entity.Fish_Management;
using main.service.Fish_Management;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    public class FishServiceInstaller : MonoInstaller<FishServiceInstaller>
    {
        [SerializeField] private int fishTotalScales;
        
        public override void InstallBindings()
        {
            Container.Bind<Fish>()
                .FromMethod(CreateFish)
                .AsSingle();
            
            Container.Bind<FishService>().AsSingle();
        }

        private Fish CreateFish(InjectContext ctx)
        {
            return new Fish(fishTotalScales);
        }
    }
}