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
    [CreateAssetMenu(fileName = "ScaleFishCE", menuName = "Data/Effects/New ScaleFishCE")]
    public class ScaleFishCE : CardEffect
    {
        [FormerlySerializedAs("message")] [SerializeField]
        private string _message;
        [SerializeField] private int _amountOfScalesBoost;
        [SerializeField] private CardClass _classToAffect;
        [SerializeField] private ItemCard _asLongAsItemOnBoard;

        public override void Execute(int multiplier)
        {
            Debug.Log("My message is " + _message);
        }
    }
}