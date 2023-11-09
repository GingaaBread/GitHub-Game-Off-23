using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Effects
{
    /// <summary>
    ///     A simple example of how classes can define card effects.
    ///     This simply defines a field "message" that will be printed upon execution.
    ///     NOTE: Can be removed once the actual card effects exist.
    /// </summary>
    [CreateAssetMenu(fileName = "ScaleFishMultipliedCE", menuName = "Data/Effects/New ScaleFishMultipliedCE")]
    public class ScaleFishMultipliedCE : CardEffect
    {
        [FormerlySerializedAs("message")] [SerializeField]
        private string _message;
        [SerializeField] private int _baseAmountOfScales;
        [SerializeField] private int _incrementAmountOfScales;
        [SerializeField] private bool _multiplyOnceForEachCardNamePlayed;
        //public Card playedCardRef;
        [SerializeField] private int _multiplicationLimit;

        public override void Execute()
        {
            Debug.Log("My message is " + _message);
        }
    }
}