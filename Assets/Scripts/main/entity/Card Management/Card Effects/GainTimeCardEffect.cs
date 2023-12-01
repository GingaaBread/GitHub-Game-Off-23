using System;
using main.entity.Card_Management.Card_Data;
using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Gain Time", menuName = "Data/Card Effect/Gain Time")]
    public class GainTimeCardEffect : CardEffect
    {
        [SerializeField] private int amountOfTimeToGain;

        private int currentAmountOfTimeToGain;

        private int CurrentAmountOfTimeToGain
        {
            get => currentAmountOfTimeToGain;
            set
            {
                currentAmountOfTimeToGain = value;
                OnEffectUpdated?.Invoke();
            }
        }
        
        public override event Action OnEffectUpdated;

        private LazyInject<TurnService> turnService;

        [Inject]
        public void Construct(LazyInject<TurnService> turnService)
        {
            this.turnService = turnService;
        }

        private void OnEnable()
        {
            ResetEffect();
        }
        
        public override void Execute()
        {
            turnService.Value.IncreaseTime(CurrentAmountOfTimeToGain);
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            CurrentAmountOfTimeToGain *= multiplier;
        }

        protected override void ResetEffect()
        {
            CurrentAmountOfTimeToGain = amountOfTimeToGain;
        }

        public override string GetDescription()
        {
            return $"Gain +{CurrentAmountOfTimeToGain} time";
        }
    }
}