using NaughtyAttributes;
using UnityEngine;

namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    ///     This entity is a card SO, which stays on board for a set duration of turns.
    /// </summary>
    [CreateAssetMenu(fileName = "Item Card", menuName = "Data/New Item Card")]
    public class ItemCard : Card
    {
        /// <summary>
        ///     Determines the amount of turns that the item card remains on the board before the effect is removed.
        /// </summary>
        [Space(20)]
        [HorizontalLine]
        [Space(20)]
        [InfoBox("Determines the amount of turns that the item card remains on the board before the " +
                 "effect is removed")]
        [SerializeField]
        private int _amountOfTurnsToLast;
    }
}