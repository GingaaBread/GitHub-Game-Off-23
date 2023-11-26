using System.Collections.Generic;
using main.entity.Card_Management.Card_Data;

namespace main.entity.Card_Management
{
    public class StarterDeck
    {
        public List<Card> Cards { get; } = new();

        public bool IsEmpty()
        {
            return Cards.Count == 0;
        }
    }
}