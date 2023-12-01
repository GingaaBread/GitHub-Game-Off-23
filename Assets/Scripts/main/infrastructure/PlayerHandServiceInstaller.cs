using main.entity.Card_Management;
using main.service.Card_Management;
using main.view;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    public class PlayerHandServiceInstaller : MonoInstaller<PlayerHandServiceInstaller>
    {
        [SerializeField] private int initialDrawAmount;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerHand>().AsSingle().WithArguments(initialDrawAmount);
            
            Container.BindInterfacesAndSelfTo<PlayerHandService>().AsSingle();
        }
    }
}