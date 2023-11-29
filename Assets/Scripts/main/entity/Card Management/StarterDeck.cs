using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Card_Management
{
    public class StarterDeck
    {
        public IReadOnlyList<Card> Cards { get; }

        public StarterDeck(IReadOnlyList<Card> cards)
        {
            Cards = cards;
        }
    }
}