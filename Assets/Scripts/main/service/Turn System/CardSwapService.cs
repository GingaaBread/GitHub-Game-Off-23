using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Card_Management;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace main.service.Turn_System
{
    public class CardSwapService : Service
    {
        private readonly Game game;
        private readonly DeckService deckService;
        private readonly CardPoolService cardPoolService;
        private readonly DiscardPileService discardPileService;
        private readonly Turn turn;

        public CardSwapService(Game game, DeckService deckService, CardPoolService cardPoolService, DiscardPileService discardPileService, Turn turn)
        {
            this.game = game;
            this.deckService = deckService;
            this.cardPoolService = cardPoolService;
            this.discardPileService = discardPileService;
            this.turn = turn;
        }
        
        /// <summary>
        ///     Triggered once the card swap has selected three cards from the players deck.
        ///     The player must choose one of these cards and remove it from the deck.
        ///     The first argument list is for the cards that are taken from the deck, where one must be removed.
        ///     The second argument list is for the cards that are offered to the player, where one must be added.
        /// </summary>
        public readonly UnityEvent<List<Card>, List<Card>> OnCardSwapOptions = new();

        public event Action OnCardsSwapped;
        
        public void HandleCardSwapOptions()
        {
            if (!CanSwapCards())
            {
                LogInfo("Because of the current turn, there will be no card swap");
                OnCardsSwapped?.Invoke();
                return;
            }
            
            LogInfo("Game is still ongoing, therefore doing the card swap now");

            // If there are more than three cards sharing the least rarity, it should be randomised
            var random = new Random();

            var maximumRarity = turn.CurrentTurnNumber;
            LogInfo($"Maximum rarity available is {maximumRarity}");

            // Randomly select 3 cards of the player's current rarity (i.e. turn number) - 2  or less
            var finalResult = deckService
                .ToList()
                .Concat(discardPileService.ToList())
                .Distinct(new CardComparer())
                .Where(card => card.Rarity <= maximumRarity - 2)
                .OrderBy(_ => random.Next())
                .Take(3)
                .ToList();

            // In any case, there should definitely be three elements now
            Assert.IsNotNull(finalResult[0]);
            Assert.IsNotNull(finalResult[1]);
            Assert.IsNotNull(finalResult[2]);

            LogInfo("The three random cards in both piles up to current rarity - 2 are: " +
                    string.Join(",", finalResult));

            LogInfo("Now randomly selecting three cards from the card pool up to the maximum rarity");
            var selectedCards = cardPoolService
                .ToList()
                .Distinct(new CardComparer())
                .Where(card => card.Rarity <= maximumRarity)
                .OrderBy(_ => random.Next())
                .Take(3)
                .ToList();

            // Verifies the game integrity, there should be exactly three cards at this point
            Assert.IsNotNull(selectedCards[0]);
            Assert.IsNotNull(selectedCards[1]);
            Assert.IsNotNull(selectedCards[2]);

            // If the player would be offered the same 3 card pool cards two turns in a row, randomly swap one of
            // the offered card pool cards with another type (either of equal rarity or the next highest rarity).
            if (game.LastOfferedCards?[0] == selectedCards[0] &&
                game.LastOfferedCards?[1] == selectedCards[1] &&
                game.LastOfferedCards?[2] == selectedCards[2])
            {
                LogInfo("The game tries to offer the exact same cards from the card pool" +
                        " as last time, therefore randomly swapping one card with another card of equal" +
                        " or next highest rarity.");

                Assert.IsNotNull(selectedCards[3], "Trying to swap cards with the next rare card, but " +
                                                   "there is none in the card pool.");

                // Do the swap with the next highest rarity card 
                var randomIndexToSwap = UnityEngine.Random.Range(0, 3);
                selectedCards[randomIndexToSwap] = selectedCards[3];
            }

            LogInfo("The three cards chosen from the pool to swap are: " +
                    $"\n- {selectedCards[0]}" +
                    $"\n- {selectedCards[1]}" +
                    $"\n- {selectedCards[2]}");

            LogInfo("Now waiting for the player to select one card to exchange");
            LogInfo("Triggering the OnCardSwap event");
            OnCardSwapOptions.Invoke(finalResult, selectedCards);

            // TODO remove player give choice from card pool

            LogInfo("Handled the card swap");
        }
        
        public void RegisterCardSwapSelections([NotNull] Card cardToRemoveFromDeck, [NotNull] Card cardToAddToDeck)
        {
            LogInfo($"Registered the selection made by the player. Removing card '{cardToRemoveFromDeck}'" +
                    $" and adding card '{cardToAddToDeck}'");
            
            deckService.ExchangeCardForAnother(cardToRemoveFromDeck, cardToAddToDeck);
            
            OnCardsSwapped?.Invoke();
        }
        
        /// <summary>
        ///     Yields true if the system should swap cards or not.
        ///     During the first three turns, the system will NOT swap cards because there are too few low-rarity cards.
        /// </summary>
        /// <returns>true - if the game can swap cards; false - if it cannot</returns>
        private bool CanSwapCards()
        {
            LogInfo("Checking if the swap system can be used or not because of the current turn");
            LogInfo($"Cards should only be swapped, starting in turn {game.TurnToStartSwappingCards}" +
                    $" until turn {game.TurnToStopSwappingCards}" +
                    $" and now is turn {turn.CurrentTurnNumber}");
            
            return turn.CurrentTurnNumber >= game.TurnToStartSwappingCards && turn.CurrentTurnNumber <= game.TurnToStopSwappingCards;
        }
    }
}