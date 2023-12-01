using System.Collections.Generic;
using UnityEngine;

namespace main.entity.Card_Management.Deck_Definition
{
    public abstract class DeckDefinition : ScriptableObject
    {
        [SerializeField] private List<CardCopies> cardCopiesList = new();

        public List<CardCopies> CardCopiesList => cardCopiesList;
    }
}