using System;
using main.entity.Card_Management.Card_Data;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    public abstract class MultiplyEffectsCardEffect : CardEffect
    {
        private int multiplier;

        protected int Multiplier
        {
            get => multiplier;
            private set
            {
                multiplier = value;
                OnEffectUpdated?.Invoke();
            }
        }
        
        public override event Action OnEffectUpdated;

        protected PlayerHand playerHand;

        [Inject]
        public void Construct(PlayerHand playerHand)
        {
            this.playerHand = playerHand;
        }

        private void OnEnable()
        {
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            Multiplier *= multiplier;
        }

        protected override void ResetEffect()
        {
            Multiplier = 2;
        }
    }
}