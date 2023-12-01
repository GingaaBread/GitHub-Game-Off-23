using System.Linq;
using main.entity.Card_Management.Card_Data;
using UnityEngine;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Multiply Effects By Class", menuName = "Data/Card Effect/Multiply Effects By Class")]
    public class MultiplyEffectsByClassCardEffect : MultiplyEffectsCardEffect
    {
        [SerializeField] private CardClass targetClass;
        
        public override void Execute()
        {
            playerHand.HandCards
                .Where(card => card.CardClass.Equals(targetClass.ToString()))
                .ToList()
                .ForEach(card => card.MultiplyEffects(Multiplier));
            
            ResetEffect();
        }
        
        public override string GetDescription()
        {
            return $"x{Multiplier} the effects of all {targetClass.ToString()} cards in your hand";
        }
    }
}