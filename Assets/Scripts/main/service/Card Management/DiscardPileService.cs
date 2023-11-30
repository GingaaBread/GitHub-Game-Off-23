using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.infrastructure;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    public class DiscardPileService : Service
    {
        private readonly CardPile discardPile;
        private readonly DeckService deckService;

        public DiscardPileService(CardPile discardPile, DeckService deckService)
        {
            this.discardPile = discardPile;
            this.deckService = deckService;
        }

        public event Action<Card> OnDiscard;

        /// <summary>
        ///     Adds the card to the top of the discard pile
        /// </summary>
        /// <param name="card">The card instance that should be pushed to the discard pile stack</param>
        public void Discard([NotNull] Card card)
        {
            LogInfo($"Discarding card '{card}'");
            
            discardPile.Pile.AddFirst(card);
            
            OnDiscard?.Invoke(card);
        }
        
        public void RemoveCard(Card card)
        {
            card.RemoveFrom(discardPile.Pile);
        }

        /// <summary>
        ///     Removes all cards from the discard pile and shuffles them randomly back into the deck
        /// </summary>
        public void ShuffleBackIntoDeck()
        {
            // The discard pile should only be shuffled back into the deck if the deck is empty
            Assert.IsTrue(deckService.IsEmpty());

            LogInfo("Shuffling the discard pile back into the deck");

            var cards = new List<Card>(discardPile.Pile);
            
            discardPile.Pile.Clear();
            
            LogInfo("Removed all cards from the discard pile");
            
            cards.Shuffle();
            
            cards.ForEach(deckService.AddCard);
        }

        /// <summary>
        ///     Yields the discard pile as a list of cards
        /// </summary>
        /// <returns>The discard pile stack converted to a list</returns>
        public IEnumerable<Card> ToList()
        {
            return discardPile.Pile.ToList();
        }

        public int GetAmountInDiscardPile(Card refCard){
            int valueToReturn = 0;
            foreach(Card pileCard in discardPile.Pile){
                if(pileCard == refCard) valueToReturn++;
            }
            return valueToReturn;
        }
    }
}