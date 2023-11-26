using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    public class TurnServiceInstaller : MonoInstaller<TurnServiceInstaller>
    {
        [SerializeField] private UnitTime turnInitialTime;
        
        public override void InstallBindings()
        {
            Container.Bind<Turn>()
                .FromMethod(CreateTurn)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<TurnService>().AsSingle();
        }
        
        private Turn CreateTurn()
        {
            return new Turn(turnInitialTime, 0, turnInitialTime);
        }
    }
}