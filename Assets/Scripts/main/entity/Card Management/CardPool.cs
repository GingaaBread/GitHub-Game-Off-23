using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     This entity is used to track discovered cards.
    ///     At the start of the game, this should be the relative component set of the card vault and the starter deck.
    /// </summary>
    public class CardPool
    {
        /// <summary>
        ///     Contains the pool of discovered cards as a list.
        ///     The instance is created automatically.
        /// </summary>
        public List<Card> Cards { get; }

        public CardPool(List<Card> cards)
        {
            Cards = cards;
        }
    }
}