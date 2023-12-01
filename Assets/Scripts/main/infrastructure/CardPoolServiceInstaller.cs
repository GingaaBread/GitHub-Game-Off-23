using System.Collections.Generic;
using System.Linq;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class CardPoolServiceInstaller : MonoInstaller<CardPoolServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CardPool>()
                .FromMethod(CreateCardPool)
                .AsSingle();
            
            Container.Bind<CardPoolService>().AsSingle();
        }

        private static CardPool CreateCardPool(InjectContext ctx)
        {
            var deckDefinitionRepository = ctx.Container.ResolveId<DeckDefinitionRepository>(ResourcePath.CardPool.GetValue());
            var deckDefinition = deckDefinitionRepository.LoadDeckDefinition();
            var starterDeck = ctx.Container.Resolve<StarterDeck>();
            var cards = new List<Card>();
            foreach (var cardCopies in deckDefinition.CardCopiesList)
            {
                var copiesInStarterDeck = starterDeck.Cards.ToList().FindAll(card => card.Name.Equals(cardCopies.Card.Name));
                
                var numberOfCopiesToAdd = cardCopies.NumberOfCopies - copiesInStarterDeck.Count;
                
                for (var i = 0; i < numberOfCopiesToAdd; i++)
                {
                    var card = ctx.Container.ResolveId<Card>(cardCopies.Card.Name);
                    var cardEffects = new List<CardEffect>();
                    card.CardEffects.ForEach(effect => cardEffects.Add(ctx.Container.ResolveId<CardEffect>(effect)));
                    card.CardEffects = cardEffects;
                    cards.Add(card);
                }
            }
            return new CardPool(cards);
        }
    }
}