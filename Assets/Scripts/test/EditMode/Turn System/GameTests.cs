using System.Collections;
using System.Collections.Generic;
using System.Linq;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.entity.Card_Management.Deck_Definition;
using main.entity.Fish_Management;
using main.entity.Turn_System;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using main.service.Card_Management;
using main.service.Fish_Management;
using main.service.Turn_System;
using UnityEngine.Localization.Settings;
using UnityEngine.TestTools;

namespace test.EditMode.Turn_System
{
    public class GameTests
    {
        [UnityTest]
        public IEnumerator Sample()
        {
            // Adds the first locale as the selected locale in CI executions 
            LocalizationSettings.SelectedLocale ??= LocalizationSettings.AvailableLocales.Locales[0];

            const int turnsInAGame = 20;
            const int turnToStartSwappingCards = 3;
            const int turnToStopSwappingCards = 17;
            var game = new Game(turnsInAGame, turnToStartSwappingCards, turnToStopSwappingCards);
            var fish = new Fish(100);
            var fishService = new FishService(fish);
            var starterDeckResourceLoader = new ResourceLoader<DeckDefinition>(ResourcePath.StarterDeck);
            var starterDeckDefinitionRepository = new DeckDefinitionRepository(starterDeckResourceLoader);
            var starterDeckDefinition = starterDeckDefinitionRepository.LoadDeckDefinition();
            var starterDeckCards = new List<Card>();
            foreach (var cardCopies in starterDeckDefinition.CardCopiesList)
            {
                for (var i = 0; i < cardCopies.NumberOfCopies; i++)
                {
                    starterDeckCards.Add(cardCopies.Card);
                }
            }
            var starterDeck = new StarterDeck(starterDeckCards);
            var deckCardPile = new CardPile(starterDeck.Cards.ToList(), true);
            var deckService = new DeckService(deckCardPile);
            var discardPileService = new DiscardPileService(new CardPile(), deckService);
            var cardPoolResourceLoader = new ResourceLoader<DeckDefinition>(ResourcePath.CardPool);
            var cardPoolDefinitionRepository = new DeckDefinitionRepository(cardPoolResourceLoader);
            var cardPoolDeckDefinition = cardPoolDefinitionRepository.LoadDeckDefinition();
            var cardPoolCards = new List<Card>();
            foreach (var cardCopies in cardPoolDeckDefinition.CardCopiesList)
            {
                var copiesInStarterDeck = starterDeck.Cards.ToList().FindAll(card => card.Equals(cardCopies.Card));
                var numberOfCopiesToAdd = cardCopies.NumberOfCopies - copiesInStarterDeck.Count;
                for (var i = 0; i < numberOfCopiesToAdd; i++)
                {
                    cardPoolCards.Add(cardCopies.Card);
                }
            }
            var cardPool = new CardPool(cardPoolCards);
            var cardPoolService = new CardPoolService(cardPool);
            var initialTime = new UnitTime { Time = 10 };
            var turn = new Turn(initialTime, 0, initialTime);
            var effectAssembly = new EffectAssembly();
            var effectAssemblyService = new EffectAssemblyService(effectAssembly);
            var playerHand = new PlayerHand(5);
            var playerHandService = new PlayerHandService(playerHand, deckService, discardPileService, turn, effectAssemblyService);
            // Create a new game
            var gameService = new GameService(game, turn, fishService);
            var cardSwapService = new CardSwapService(game, deckService, cardPoolService, discardPileService, turn);
            var turnService = new TurnService(turn, playerHandService, gameService, cardSwapService, effectAssemblyService);
            
            while (gameService.GameIsRunningJustForTest)
            {
                turnService.StartTurn();
                // As a test, only the first card is always played
                playerHandService.PlayCard(playerHand.HandCards[0]);
                turnService.EndTurn();
            }

            yield return null;
        }
    }
}