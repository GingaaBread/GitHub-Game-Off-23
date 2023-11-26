using main.entity.Turn_System;
using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    public class GameServiceInstaller : MonoInstaller<GameServiceInstaller>
    {
        [SerializeField] private int turnsInAGame;
        [SerializeField] private int turnToStartSwappingCards;
        [SerializeField] private int turnToStopSwappingCards;
        
        public override void InstallBindings()
        {
            Container.Bind<Game>()
                .FromMethod(CreateGame)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<GameService>().AsSingle();
        }

        private Game CreateGame(InjectContext ctx)
        {
            return new Game(turnsInAGame, turnToStartSwappingCards, turnToStopSwappingCards);
        }
    }
}