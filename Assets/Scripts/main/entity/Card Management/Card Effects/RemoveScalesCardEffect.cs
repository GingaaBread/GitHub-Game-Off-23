using System;
using main.entity.Card_Management.Card_Data;
using main.service.Fish_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Remove Scales", menuName = "Data/Card Effect/Remove Scales")]
    public class RemoveScalesCardEffect : CardEffect, IPreviewable
    {
        [SerializeField] private int amountOfScalesToRemove;

        protected int currentAmountOfScalesToRemove;

        public override event Action OnEffectUpdated;

        private FishService fishService;

        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        private void OnEnable()
        {
            ResetEffect();
        }
        
        public override void Execute()
        {
            fishService.ScaleFish(GetCurrentAmountOfScalesToRemove());
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            var amount = GetCurrentAmountOfScalesToRemove() * multiplier;
            SetCurrentAmountOfScalesToRemove(amount);
        }

        protected override void ResetEffect()
        {
            SetCurrentAmountOfScalesToRemove(amountOfScalesToRemove);
        }

        public override string GetDescription()
        {
            return $"Remove {GetCurrentAmountOfScalesToRemove()} scales";
        }

        public int PreviewAmount()
        {
            return GetCurrentAmountOfScalesToRemove();
        }
        
        protected virtual int GetCurrentAmountOfScalesToRemove()
        {
            return currentAmountOfScalesToRemove;
        }
        
        private void SetCurrentAmountOfScalesToRemove(int amount)
        {
            currentAmountOfScalesToRemove = amount;
            OnEffectUpdated?.Invoke();
        }
    }
}