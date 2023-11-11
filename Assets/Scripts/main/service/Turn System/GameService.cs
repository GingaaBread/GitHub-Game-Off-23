﻿using System;
using System.Linq;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Card_Management;
using UnityEngine.Assertions;

namespace main.service.Turn_System
{
    /// <summary>
    ///     This service provides the business logic for the game entity, including a way to end the current turn.
    ///     TODO: I feel like this class is responsible for too much, let's review it!
    /// </summary>
    public class GameService : Service
    {
        /// <summary>
        ///     The non-null game entity created automatically when the service is instantiated.
        /// </summary>
        [NotNull] private readonly Game _game = new();

        /// <summary>
        ///     Creates the singleton of this service if it does not exist and then starts the game
        /// </summary>
        public GameService()
        {
            Instance ??= this;
            LogInfo("Successfully set the GameService's singleton instance");

            StartNewGame();
        }

        /// <summary>
        ///     The non-thread-safe singleton of the service
        /// </summary>
        public static GameService Instance { get; private set; }

        /// <summary>
        ///     Ends the current turn when the end turn button view is clicked.
        /// </summary>
        public void EndTurn()
        {
            LogInfo("Now ending the current turn");
            Assert.IsTrue(_game.currentGameState is GameState.PLAY_CARDS, "Should currently be in the play" +
                                                                          "cards state!");

            // First, the turn is ended
            _game.currentGameState = GameState.TURN_END;
            // TODO: Unity View Stuff (animations, etc.)

            // Now apply all end-of-turn effects
            _game.currentGameState = GameState.END_OF_TURN_EFFECT_EXECUTION;
            LogInfo("Now executing all end of turn effects");
            EffectAssemblyService.Instance.ExecuteAll();

            // If the game is now over, handle the end of the run
            if (GameIsOver()) HandleGameOver();

            // If the game is not yet over, handle the card swap system
            else HandleCardSwaps();

            // Increment tracking variables
            _game.elapsedTurns++;
            LogInfo($"Incrementing turn number. It now is: {_game.elapsedTurns}");
            // TODO: Refresh available time

            // At the end, start the new turn
            _game.currentGameState = GameState.TURN_START;
            // TODO: Do view stuff 
        }

        /// <summary>
        ///     Starts the current game by creating all required services (resetting the old ones if they existed
        ///     already), loading the deck, pool, discard pile, etc. and then drawing the starter hand
        /// </summary>
        private void StartNewGame()
        {
            LogInfo("Now starting a new game");

            CreateServices();
            LoadDeck();
            DrawStartingHand();

            LogInfo("Successfully started the game, now waiting for player actions");

            // Reset the game variables
            _game.fishHasBeenScaledThisOrLastTurn = true;
            _game.currentGameState = GameState.PLAY_CARDS;
        }

        /// <summary>
        ///     Creates the singletons of all service classes
        /// </summary>
        private void CreateServices()
        {
            LogInfo("Now creating all service singleton instances");

            new EffectAssemblyService();
            LogInfo("EffectAssemblyService has been instantiated");

            new CardVaultService();
            LogInfo("CardVaultService has been instantiated");

            new DeckService();
            LogInfo("DeckService has been instantiated");

            new CardPoolService();
            LogInfo("CardPoolService has been instantiated");

            new PlayerHandService();
            LogInfo("PlayerHandService has been instantiated");

            new DiscardPileService();
            LogInfo("DiscardPileService has been instantiated");

            LogInfo("Successfully created all services");
        }

        /// <summary>
        ///     Loads the deck piles and sets up the card pool
        /// </summary>
        private void LoadDeck()
        {
            LogInfo("Now loading the deck collections");

            // Fill the card pool with all cards from the vault
            LogInfo("Filling the card pool with all cards from the vault");
            var cardsInVault = CardVaultService.Instance.GetAll();
            foreach (var card in cardsInVault)
                for (var i = 0; i < card.NumberOfCopiesInPool; i++)
                    CardPoolService.Instance.AddCard(card);
            LogInfo($"In total, there are {CardPoolService.Instance.Size()} cards in the pool");

            // Remove the starter deck cards from the card pool
            LogInfo("Removing all cards from the starter deck from the card pool");
            var cardsInStarterDeck = DeckService.Instance.ToList();
            foreach (var card in cardsInStarterDeck) CardPoolService.Instance.RemoveCard(card);
            LogInfo($"After removal, in total, there are {CardPoolService.Instance.Size()} cards in the pool");
        }

        /// <summary>
        ///     Draws the required amount of cards the player starts with
        /// </summary>
        private void DrawStartingHand()
        {
            LogInfo("Now drawing starting hand");
            PlayerHandService.Instance.Draw(5);
        }

        private void HandleGameOver()
        {
            Assert.IsTrue(_game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check state!");

            LogInfo("Game over. Now ending the game");
            _game.currentGameState = GameState.GAME_OVER;

            // TODO: Do the roguelike end of game stuff from the diagram
        }

        private void HandleCardSwaps()
        {
            Assert.IsTrue(_game.currentGameState is GameState.GAME_OVER_CHECK,
                "Should currently be in the game over check state!");
            _game.currentGameState = GameState.CARDS_SWAP;

            LogInfo("Game is still ongoing, therefore doing the card swap now");

            // First, discarding all hand cards
            PlayerHandService.Instance.DiscardHand();

            // If there are more than three cards sharing the least rarity, it should be randomised
            var random = new Random();

            /**
             * TODO
             * If the player would be offered the same 3 card pool cards two turns in a row, randomly swap one of the
             * offered card pool cards with another type (either of equal rarity or the next highest rarity).
             */

            // Getting the lowest rarity cards from the deck, whilst ignoring duplicates
            // Note that there are less than three cards in the deck, one or more values will be null.
            var deckResult = DeckService
                .Instance
                .ToList()
                .Distinct(new CardComparer())
                .OrderBy(_ => random.Next())
                .ThenBy(it => it.Rarity)
                .Take(3)
                .ToList();

            // Now doing the same for the discard pile
            var discardPileResult = DiscardPileService
                .Instance
                .ToList()
                .Distinct(new CardComparer())
                .OrderBy(_ => random.Next())
                .ThenBy(it => it.Rarity)
                .Take(3)
                .ToList();

            // Now combining both result sets, whilst again ignoring duplicates, and taking the three least rare cards
            deckResult.AddRange(discardPileResult);
            var finalResult = deckResult
                .Distinct(new CardComparer())
                .OrderBy(_ => random.Next())
                .ThenBy(it => it.Rarity)
                .Take(3)
                .ToList();

            // In any case, there should definitely be three elements now
            Assert.IsNotNull(finalResult[0]);
            Assert.IsNotNull(finalResult[1]);
            Assert.IsNotNull(finalResult[2]);

            LogInfo("The three lowest rarity cards to swap now are: " +
                    $"\n- {finalResult[0]}" +
                    $"\n- {finalResult[1]}" +
                    $"\n- {finalResult[2]}");

            var maximumRarity = _game.elapsedTurns;
            LogInfo($"Maximum rarity available is {maximumRarity}");

            LogInfo("Now randomly selecting three cards from the card pool up to the maximum rarity");
            var selectedCards = CardPoolService
                .Instance
                .ToList()
                .Distinct(new CardComparer())
                .Where(card => card.Rarity <= maximumRarity)
                .OrderBy(_ => random.Next())
                .Take(3)
                .ToList();

            Assert.IsNotNull(selectedCards[0]);
            Assert.IsNotNull(selectedCards[1]);
            Assert.IsNotNull(selectedCards[2]);

            LogInfo("The three cards chosen from the pool to swap are: " +
                    $"\n- {selectedCards[0]}" +
                    $"\n- {selectedCards[1]}" +
                    $"\n- {selectedCards[2]}");

            LogInfo("Now waiting for the player to select one card to exchange");
            // TODO impl the player selection

            LogInfo("Handled the card swap");
        }

        /// <summary>
        ///     Yield true if the game is over and false if it is not, and the game should continue normally.
        ///     A game is considered over if there have been no scaled fish for at least two turns or if the maximum
        ///     turns in a game has been reached (or exceeded).
        /// </summary>
        /// <returns>true - if the game is over; false - if the game is not yet over</returns>
        private bool GameIsOver()
        {
            LogInfo("Now checking if the game is over");
            _game.currentGameState = GameState.GAME_OVER_CHECK;
            return !_game.fishHasBeenScaledThisOrLastTurn || _game.elapsedTurns >= Game.TURNS_IN_A_GAME;
        }
    }
}