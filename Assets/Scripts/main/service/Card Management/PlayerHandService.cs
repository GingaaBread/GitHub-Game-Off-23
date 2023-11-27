﻿using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Turn_System;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This services provides the business logic for the player hand entity, which allows drawing cards,
    ///     playing them, etc.
    /// </summary>
    public class PlayerHandService : Service
    {
        private readonly PlayerHand playerHand;
        private readonly DeckService deckService;
        private readonly DiscardPileService discardPileService;
        private readonly Turn turn;
        private readonly EffectAssemblyService effectAssemblyService;

        /// <summary>
        ///     Triggered when a new card has been drawn.
        ///     If multiple cards are drawn, this event is triggered once per each card.
        /// </summary>
        public readonly UnityEvent<Card> OnCardDrawn = new();
        
        public PlayerHandService(PlayerHand playerHand, DeckService deckService, DiscardPileService discardPileService, Turn turn, EffectAssemblyService effectAssemblyService)
        {
            this.playerHand = playerHand;
            this.deckService = deckService;
            this.discardPileService = discardPileService;
            this.turn = turn;
            this.effectAssemblyService = effectAssemblyService;
        }
        
        /// <summary>
        ///     Draws the amount of cards specified in DrawAmount from PlayerHand. If the amount is larger than the
        ///     amount of cards left in the deck, all cards from the discard pile will be shuffled back into the deck
        ///     and the remaining cards will be drawn from the newly shuffled deck.
        /// </summary>
        public void Draw()
        {
            LogInfo($"Drawing {playerHand.DrawAmount} card(s)");
            var amountOfCardsInDeck = deckService.Size();

            // TODO: new shuffle
            // TODO: if the deck is empty and discard pile are empty, just return out
            // Does the deck need to be refilled and reshuffled?
            if (playerHand.DrawAmount > amountOfCardsInDeck)
            {
                var remainingCardsToDrawAfterDrawingLastCardsFromDeck = playerHand.DrawAmount - amountOfCardsInDeck;

                // Draw all remaining cards from the deck
                DrawCardsFromDeck(amountOfCardsInDeck);

                // Now refill the deck and shuffle it
                discardPileService.ShuffleBackIntoDeck();

                // Now draw the remaining amount of cards
                DrawCardsFromDeck(remainingCardsToDrawAfterDrawingLastCardsFromDeck);
            }
            // If the deck has enough cards, just draw them
            else
            {
                DrawCardsFromDeck(playerHand.DrawAmount);
            }
        }
        
        public void PlayCard(Card card)
        {
            Assert.IsTrue(playerHand.HandCards.Contains(card), 
                $"Cannot play card '{card}' because it is not in the player's hand.");
            
            if (!CardHasEnoughTime(card))
            {
                LogInfo($"Not enough time to playing card '{card}'");
                return;
            }

            LogInfo($"Playing card '{card}'");

            card.CardEffects.ForEach(effectAssemblyService.AddEffect);

            playerHand.HandCards.Remove(card);
            
            discardPileService.Discard(card);

            LogInfo("Successfully played the card");
        }
        
        /// <summary>
        ///     Removes all cards from the player's hand and adds them to the discard pile
        /// </summary>
        public void DiscardHand()
        {
            LogInfo("Discarding the entire player hand");

            playerHand.HandCards.ForEach(discardPileService.Discard);
            
            playerHand.HandCards.Clear();
        }
        
        /// <summary>
        ///     Helper method that will "actually" draw the cards from the deck and then add them to the hand.
        ///     All assertions, state checks and so on should be done by the caller!
        /// </summary>
        /// <param name="amount">The amount of cards to draw</param>
        private void DrawCardsFromDeck(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var drawnCard = deckService.DrawFromTop();
                playerHand.HandCards.Add(drawnCard);

                OnCardDrawn.Invoke(drawnCard);
                
                LogInfo("Triggered the OnCardDrawn event");
            }
        }
        
        private bool CardHasEnoughTime(Card card)
        {
            return card.TimeCost <= turn.RemainingTime.Time;
        }
    }
}