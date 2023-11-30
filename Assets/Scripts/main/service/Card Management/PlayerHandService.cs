using System.Collections.Generic;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.entity.Card_Management.Card_Effects;
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
        private readonly DeckService deckService;
        private readonly DiscardPileService discardPileService;
        private readonly EffectAssemblyService effectAssemblyService;

        /// <summary>
        ///     Triggered when a new card has been drawn.
        ///     If multiple cards are drawn, this event is triggered once per each card.
        /// </summary>
        public readonly UnityEvent<Card> OnCardDrawn = new();

        public readonly UnityEvent<int> OnTimeUnitChange = new();

        private readonly PlayerHand playerHand;
        public readonly UnityEvent<int> ScaleCounterShouldIncrease = new();
        private readonly Turn turn;
        public List<PlayedCardCounter> playedCardCounter;

        public PlayerHandService(PlayerHand playerHand, DeckService deckService, DiscardPileService discardPileService,
            Turn turn, EffectAssemblyService effectAssemblyService)
        {
            this.playerHand = playerHand;
            this.deckService = deckService;
            this.discardPileService = discardPileService;
            this.turn = turn;
            this.effectAssemblyService = effectAssemblyService;

            playedCardCounter = new List<PlayedCardCounter>();
        }

        public void StartTurnDraw()
        {
            Draw();
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

            LogInfo("Initial time before was " + turn.InitialTime.Time);
            turn.RemainingTime.Time -= card.TimeCost;

            LogInfo($"Removing {card.TimeCost} time, time is now {turn.RemainingTime.Time}");
            OnTimeUnitChange.Invoke(turn.RemainingTime.Time);

            LogInfo("Initial time after is " + turn.InitialTime.Time);

            LogInfo($"Playing card '{card}'");

            if (playedCardCounter.Count < 1)
            {
                LogInfo("Empty counter, creating list item");
                playedCardCounter.Add(new PlayedCardCounter(card.Name));
            }
            else
            {
                var containsCard = false;
                foreach (var playedCard in playedCardCounter)
                    if (playedCard.CardName() == card.Name)
                    {
                        containsCard = true;
                        playedCard.IncrementAmount();
                        LogInfo("Card found in list, incrementing");
                    }

                if (!containsCard)
                {
                    LogInfo("Card not yet in list, creating list item");
                    playedCardCounter.Add(new PlayedCardCounter(card.Name));
                }
            }

            foreach (var cardEffect in card.CardEffects)
            {
                if (cardEffect.GetType() == typeof(RemoveScalesCardEffect))
                {
                    var estimatedEffect = cardEffect as RemoveScalesCardEffect;
                    var estimatedScalesRemoved = estimatedEffect.AmountOfScalesToRemove() * card.Multiplier;
                    ScaleCounterShouldIncrease.Invoke(estimatedScalesRemoved);
                }
                else if (cardEffect.GetType() == typeof(ScaleFishMultipliedCE))
                {
                    var estimatedEffect = cardEffect as ScaleFishMultipliedCE;
                    var estimatedScalesRemoved = estimatedEffect.EstimateAmountOfScalesToRemove() * card.Multiplier;
                    ScaleCounterShouldIncrease.Invoke(estimatedScalesRemoved);
                }

                effectAssemblyService.AddEffect(card.Multiplier, cardEffect);
            }

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

        public bool CardHasEnoughTime(Card card)
        {
            return card.TimeCost <= turn.RemainingTime.Time;
        }

        public void ResetPlayedCardCounter()
        {
            playedCardCounter = new List<PlayedCardCounter>();
        }

        public int RemainingTime()
        {
            return turn.RemainingTime.Time;
        }

        public int RemainingCards()
        {
            return playerHand.HandCards.Count;
        }
    }
}