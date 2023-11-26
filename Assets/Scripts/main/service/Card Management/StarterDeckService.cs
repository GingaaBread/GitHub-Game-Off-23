using System.Linq;
using main.entity.Card_Management;
using main.repository.Card_Management.Deck_Definition;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    public class StarterDeckService : Service
    {
        private readonly StarterDeck starterDeck;
        private readonly DeckDefinitionRepository deckDefinitionRepository;

        public StarterDeckService(StarterDeck starterDeck, DeckDefinitionRepository deckDefinitionRepository)
        {
            this.starterDeck = starterDeck;
            this.deckDefinitionRepository = deckDefinitionRepository;
            SetStarterDeck();
        }

        private void SetStarterDeck()
        {
            Assert.IsTrue(starterDeck.IsEmpty(), "Expected Starter Deck to be empty.");
            
            var deckDefinition = deckDefinitionRepository.LoadDeckDefinition();

            foreach (var cardCopies in deckDefinition.CardCopiesList)
            {
                starterDeck.Cards.AddRange(Enumerable.Repeat(cardCopies.Card, cardCopies.NumberOfCopies));
            }
        }
    }
}