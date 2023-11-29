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
    [CreateAssetMenu(fileName = "DoubleEffectCE", menuName = "Data/Effects/New DoubleEffectCE")]
    public class DoubleEffectCE : CardEffect
    {
        private Card _handCardTarget; //not serialized because it is only set at runtime
        [SerializeField] private bool _affectsAllCardsThisTurn;
        [SerializeField] private CardClass _classToAffect;
        [SerializeField] private bool _affectsActionCards;
        [SerializeField] private bool _affectsItemCards;

        public override void Execute()
        {
        }
    }
}