using System.Collections.Generic;
using JetBrains.Annotations;
using main.entity.Card_Management.Card_Data;
using main.infrastructure;
using UnityEngine.Assertions;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     The card pile entity is used for the player's deck, the discard pile, and other similar
    ///     card piles. It consists of a non-null stack of cards.
    /// </summary>
    public class CardPile
    {
        /// <summary>
        ///     The non-null stack containing the cards of the pile.
        ///     The pile can only be accessed, but not rewritten.
        ///     A new empty stack is created automatically.
        /// </summary>
        /// <returns>The non-null stack containing all cards in the pile</returns>
        public LinkedList<Card> Pile { get; } = new();
        
        /// <summary>
        ///     Creates a new empty pile
        /// </summary>
        public CardPile()
        {
            
        }
        
        /// <summary>
        ///     Provides a way to create a new card pile and instantly fill it with a set amount of cards
        /// </summary>
        /// <param name="cardsToFill">The non-null, non-empty list of cards to fill it with</param>
        /// <param name="shuffle">Determines if the content added to the pile should be randomly added or not</param>
        public CardPile([NotNull] List<Card> cardsToFill, bool shuffle)
        {
            Assert.IsTrue(cardsToFill.Count > 0, "Should not try to fill card pile with an empty list.");
            if (shuffle)
            {
                cardsToFill.Shuffle();
            }
            cardsToFill.ForEach(card => Pile.AddFirst(card));
        }
    }
}