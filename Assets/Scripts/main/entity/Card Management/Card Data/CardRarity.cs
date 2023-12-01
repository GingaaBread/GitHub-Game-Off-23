using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    ///     Defines the rarity of a card as its own entity.
    ///     The rarity defines when a card can be played: cards of rarity one can be played in round one, cards of
    ///     rarity 4 in round 4, etc.
    /// </summary>
    [Serializable]
    public class CardRarity
    {
        /// <summary>
        ///     Determines the rarity from a scale from zero to 15 (feel free to change the range)
        /// </summary>
        [FormerlySerializedAs("rarity")] [Range(0, 15)]
        public int _rarity;

        /// <summary>
        ///     Yields the rarity as an integer
        /// </summary>
        public int Rarity => _rarity;
    }
}