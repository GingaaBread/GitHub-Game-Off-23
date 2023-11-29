using main.entity.Card_Management.Card_Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Effects
{
    /// <summary>
    ///     A simple example of how classes can define card effects.
    ///     This simply defines a field "message" that will be printed upon execution.
    ///     NOTE: Can be removed once the actual card effects exist.
    /// </summary>
    [CreateAssetMenu(fileName = "ScaleFishWithHandCardSizeBonusCE", menuName = "Data/Effects/New ScaleFishWithHandCardSizeBonusCE")]
    public class ScaleFishWithHandCardSizeBonusCE : CardEffect
    {
        [SerializeField] private int _baseAmountOfScales;
        [SerializeField] private int _bonusAmountForEachCardInHand;

        public override void Execute()
        {
        }
    }
}