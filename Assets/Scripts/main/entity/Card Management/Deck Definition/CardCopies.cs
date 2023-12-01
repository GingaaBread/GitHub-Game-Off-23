using System;
using main.entity.Card_Management.Card_Data;
using UnityEngine;

namespace main.entity.Card_Management.Deck_Definition
{
    /// <summary>
    ///     Provides a way to use a serialized pair of a card reference and the amount of times that card should be
    ///     added to the deck
    /// </summary>
    [Serializable]
    public struct CardCopies
    {
        /// <summary>
        ///     Reference to a card that should be in the deck
        /// </summary>
        [Tooltip("Reference to a card that should be in the deck")]
        [SerializeField]
        private Card card;

        public Card Card => card;
        
        /// <summary>
        ///     The amount of times the card should be added to the deck
        /// </summary>
        [Tooltip("The amount of times the card should be added to the deck")]
        [SerializeField]
        private int numberOfCopies;

        public int NumberOfCopies => numberOfCopies;

        public CardCopies(Card card, int numberOfCopies)
        {
            this.card = card;
            this.numberOfCopies = numberOfCopies;
        }
    }
}