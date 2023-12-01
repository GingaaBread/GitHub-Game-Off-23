using System.Linq;
using main.entity.Card_Management;
using main.service.Card_Management;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    public class DeckServiceInstaller : MonoInstaller<DeckServiceInstaller>
    {
        [SerializeField] private bool shuffleOnCreation;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DeckService>()
                .FromMethod(CreateDeckService)
                .AsSingle();
        }

        private DeckService CreateDeckService(InjectContext ctx)
        {
            var starterDeck = ctx.Container.Resolve<StarterDeck>();
            var cardPile = new CardPile(starterDeck.Cards.ToList(), shuffleOnCreation);
            return new DeckService(cardPile);
        }
    }
}