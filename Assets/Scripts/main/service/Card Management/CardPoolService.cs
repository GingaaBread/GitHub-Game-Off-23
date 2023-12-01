using System.Collections.Generic;
using JetBrains.Annotations;
using main.entity.Card_Management;
using main.entity.Card_Management.Card_Data;

namespace main.service.Card_Management
{
    /// <summary>
    ///     This services provides the business logic for the card pool entity, which stores available cards.
    ///     At the start of the game, it should load all cards from the card vault and add them to the pool.
    ///     Then, all cards from the starter deck should be removed from the card pool.
    /// </summary>
    public class CardPoolService : Service
    {
        /// <summary>
        ///     The card pool entity containing discovered / available cards
        /// </summary>
        private readonly CardPool cardPool;
        
        public CardPoolService(CardPool cardPool)
        {
            this.cardPool = cardPool;
        }
        
        /// <summary>
        ///     Adds a non-null card to the pool of discovered cards
        /// </summary>
        /// <param name="cardToAdd">The non-null that should be added to the pool</param>
        public void AddCard([NotNull] Card cardToAdd)
        {
            cardPool.Cards.Add(cardToAdd);
            LogInfo($"Added card '{cardToAdd}' to the card pool");
        }

        /// <summary>
        ///     Removes a non-null card instance from the pool of discovered cards
        /// </summary>
        /// <param name="card">The non-null card reference to remove</param>
        public void RemoveCard([NotNull] Card card)
        {
            card.RemoveFrom(cardPool.Cards);
        }

        /// <summary>
        ///     Yields the card pool as a newly copied list
        /// </summary>
        /// <returns>a copy of the card pool list</returns>
        public IEnumerable<Card> ToList()
        {
            return new List<Card>(cardPool.Cards);
        }

        /// <summary>
        ///     Yields the amount of cards in the pool
        /// </summary>
        /// <returns>The amount of cards as an integer</returns>
        public int Size()
        {
            return cardPool.Cards.Count;
        }
    }
}