using System.Collections.Generic;

namespace main.entity.Card_Management.Card_Data
{
    public class CardComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Name == y.Name;
        }

        public int GetHashCode(Card card)
        {
            return card.Name.GetHashCode();
        }
    }
}