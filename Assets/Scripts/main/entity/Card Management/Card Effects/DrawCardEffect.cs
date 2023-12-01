using System;
using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Draw Card", menuName = "Data/Card Effect/Draw Card")]
    public class DrawCardEffect : CardEffect
    {
        [SerializeField] private int amountOfCardsToDraw;

        private int currentAmountOfCardsToDraw;

        private int CurrentAmountOfCardsToDraw
        {
            get => currentAmountOfCardsToDraw;
            set
            {
                currentAmountOfCardsToDraw = value;
                OnEffectUpdated?.Invoke();
            }
        }
        
        public override event Action OnEffectUpdated;
        
        private LazyInject<PlayerHandService> playerHandService;

        [Inject]
        public void Construct(LazyInject<PlayerHandService> playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        private void OnEnable()
        {
            ResetEffect();
        }
        
        public override void Execute()
        {
            playerHandService.Value.Draw(CurrentAmountOfCardsToDraw);
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            CurrentAmountOfCardsToDraw *= multiplier;
        }

        protected override void ResetEffect()
        {
            CurrentAmountOfCardsToDraw = amountOfCardsToDraw;
        }

        public override string GetDescription()
        {
            var cardOrCards = CurrentAmountOfCardsToDraw == 1 ? "card" : "cards";
            return $"Draw {CurrentAmountOfCardsToDraw} {cardOrCards}";
        }
    }
}