﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;
using main.infrastructure;
using UnityEngine.Assertions;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This services provides the business logic for the deck entity, represented as a card pile.
    ///     The deck is created and shuffled automatically by using the starter deck definition entity.
    /// </summary>
    public class DeckService : Service
    {
        /// <summary>
        ///     Contains the deck of the player at all points in the game.
        ///     This is created automatically once the service is instantiated, and loads the starter deck as it is
        ///     defined in the editor in a random order (shuffled).
        /// </summary>
        private readonly CardPile deck;

        public DeckService(CardPile deck)
        {
            this.deck = deck;
        }
        
        /// <summary>
        ///     Yields the deck as a list of cards
        /// </summary>
        /// <returns>The deck of cards converted to a list</returns>
        public IEnumerable<Card> ToList()
        {
            return deck.Pile.ToList();
        }

        /// <summary>
        ///     Removes the card at the top of the deck and returns it.
        /// </summary>
        /// <returns>the card at the top of the deck as a <see cref="Card" /></returns>
        public Card DrawFromTop()
        {
            Assert.IsTrue(deck.Pile.Count > 0, "Should never try to draw when the deck is empty. " +
                                                "Classes should check this first");

            var topCard = deck.Pile.First();
            deck.Pile.RemoveFirst();
            LogInfo($"Drew '{topCard}' as the top card");

            return topCard;
        }

        /// <summary>
        ///     Adds a card to the top of the deck
        /// </summary>
        /// <param name="card">The non-null instance of the <see cref="Card" /> that should be added to the deck</param>
        public void AddCard([NotNull] Card card)
        {
            LogInfo($"Added card '{card}' to the deck");
            deck.Pile.AddFirst(card);
        }
        
        public void RemoveCard(Card card)
        {
            card.RemoveFrom(deck.Pile);
        }

        /// <summary>
        ///     Yields the amount of cards in the deck
        /// </summary>
        /// <returns>The size as an integer</returns>
        public int Size()
        {
            return deck.Pile.Count;
        }

        /// <summary>
        ///     Utility method to check if the deck is empty or not.
        ///     Note that this is just syntactic sugar for checking if the size method yields zero.
        /// </summary>
        /// <returns>true - if the deck is empty; false - if the deck contains at least one card</returns>
        public bool IsEmpty()
        {
            return Size() is 0;
        }

        public void ShuffleDeck()
        {
            var cards = new List<Card>(deck.Pile);
            deck.Pile.Clear();
            cards.Shuffle();
            cards.ForEach(card => deck.Pile.AddFirst(card));
        }
    }
}