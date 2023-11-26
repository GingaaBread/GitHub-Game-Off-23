using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Card_Management
{
    /// <summary>
    ///     The PlayerHand entity contains the list of cards that the player holds in their hand.
    ///     A new list is instantiated when the PlayerHand object is created, allowing us to simply
    ///     create a new PlayerHand instance whenever a new game is started.
    /// </summary>
    public class PlayerHand
    {
        /// <summary>
        ///     The non-null list of hand cards.
        ///     Cannot be set manually.
        /// </summary>
        /// <value>The current list of all cards the player holds in their hand</value>
        public List<Card> HandCards { get; } = new();
        
        public int DrawAmount { get; set; }

        public PlayerHand(int drawAmount)
        {
            DrawAmount = drawAmount;
        }
    }
}