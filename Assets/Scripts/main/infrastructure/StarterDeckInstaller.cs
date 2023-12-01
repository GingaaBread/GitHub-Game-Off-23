using System.Collections.Generic;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using Zenject;

namespace main.infrastructure
{
    public class StarterDeckInstaller : MonoInstaller<StarterDeckInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<StarterDeck>()
                .FromMethod(CreateStarterDeck)
                .AsSingle();
        }

        private static StarterDeck CreateStarterDeck(InjectContext ctx)
        {
            var deckDefinitionRepository = ctx.Container.ResolveId<DeckDefinitionRepository>(ResourcePath.StarterDeck.GetValue());
            var deckDefinition = deckDefinitionRepository.LoadDeckDefinition();
            var cards = new List<Card>();
            foreach (var cardCopies in deckDefinition.CardCopiesList)
            {
                for (var i = 0; i < cardCopies.NumberOfCopies; i++)
                {
                    var card = ctx.Container.ResolveId<Card>(cardCopies.Card.Name);
                    var cardEffects = new List<CardEffect>();
                    card.CardEffects.ForEach(effect => cardEffects.Add(ctx.Container.ResolveId<CardEffect>(effect)));
                    card.CardEffects = cardEffects;
                    cards.Add(card);
                }
            }
            return new StarterDeck(cards);
        }
    }
}