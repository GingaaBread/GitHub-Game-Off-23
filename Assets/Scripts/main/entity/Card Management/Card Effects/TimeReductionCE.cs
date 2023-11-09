using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Effects
{
    /// <summary>
    ///     A simple example of how classes can define card effects.
    ///     This simply defines a field "message" that will be printed upon execution.
    ///     NOTE: Can be removed once the actual card effects exist.
    /// </summary>
    [CreateAssetMenu(fileName = "TimeReductionCE", menuName = "Data/Effects/New TimeReductionCE")]
    public class TimeReductionCE : CardEffect
    {
        [FormerlySerializedAs("message")] [SerializeField]
        private string _message;

        [SerializeField] private int _reductionAmount;
        [SerializeField] private int _halfAmountInstead;
        //public CardClass requiredCardClass;
        //public T requiredCardType;
        //public T amountOfCardsToAffect

        public override void Execute()
        {
            Debug.Log("My message is " + _message);
        }
    }
}