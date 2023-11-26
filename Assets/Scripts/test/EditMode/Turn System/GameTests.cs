using System.Collections;
using System.Collections.Generic;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.entity.Card_Management.Deck_Definition;
using main.entity.Fish_Management;
using main.entity.Turn_System;
using main.infrastructure;
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
            var starterDeck = new StarterDeck();
            var localizationSettingsWrapper = new LocalizationSettingsWrapper();
            var starterDeckResourceLoader = new ResourceLoader<DeckDefinition>(localizationSettingsWrapper, ResourcePath.StarterDeck);
            var starterDeckDefinitionRepository = new DeckDefinitionRepository(starterDeckResourceLoader);
            var starterDeckService = new StarterDeckService(starterDeck, starterDeckDefinitionRepository);
            var deckService = new DeckService(new CardPile(), starterDeck);
            var discardPileService = new DiscardPileService(new CardPile(), deckService);
            var cardPoolResourceLoader = new ResourceLoader<DeckDefinition>(localizationSettingsWrapper, ResourcePath.CardPool);
            var cardPoolDefinitionRepository = new DeckDefinitionRepository(cardPoolResourceLoader);
            var cardPool = new CardPool();
            var cardPoolService = new CardPoolService(cardPool, cardPoolDefinitionRepository, starterDeck);
            var initialTime = new UnitTime { Time = 10 };
            var turn = new Turn(initialTime, 0, initialTime);
            var playerHand = new PlayerHand(5);
            var playerHandService = new PlayerHandService(playerHand, deckService, discardPileService, turn);
            var effectAssembly = new EffectAssembly();
            var effectAssemblyService = new EffectAssemblyService(effectAssembly);
            // Create a new game
            var gameService = new GameService(game, turn, fishService);
            var cardSwapService = new CardSwapService(game, deckService, cardPoolService, discardPileService, turn);
            var turnService = new TurnService(turn, playerHandService, gameService, cardSwapService, effectAssemblyService);
            deckService.Initialize();
            
            while (gameService.GameIsRunningJustForTest)
            {
                turnService.StartTurn();
                // As a test, only the first card is always played
                playerHandService.PlayCardAt(0);
                turnService.EndTurn();
            }

            yield return null;
        }
    }
}