using System.Linq;
using main.entity.Card_Management.Card_Data;
using UnityEngine;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Multiply Effects By Card Name", menuName = "Data/Card Effect/Multiply Effects By Card Name")]
    public class MultiplyEffectsByCardNameCardEffect : MultiplyEffectsCardEffect
    {
        [SerializeField] private Card targetCard;
        
        public override void Execute()
        {
            playerHand.HandCards
                .Where(card => card.Name.Equals(targetCard.Name))
                .ToList()
                .ForEach(card => card.MultiplyEffects(Multiplier));
            
            ResetEffect();
        }
        
        public override string GetDescription()
        {
            return $"x{Multiplier} the effects of all {targetCard.Name} cards in your hand";
        }
    }
}