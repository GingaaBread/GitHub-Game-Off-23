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

        private int currentAmountOfScalesToRemove;

        private int CurrentAmountOfScalesToRemove
        {
            get => currentAmountOfScalesToRemove;
            set
            {
                currentAmountOfScalesToRemove = value;
                OnEffectUpdated?.Invoke();
            }
        }
        
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
            fishService.ScaleFish(CurrentAmountOfScalesToRemove);
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            CurrentAmountOfScalesToRemove *= multiplier;
        }

        protected override void ResetEffect()
        {
            CurrentAmountOfScalesToRemove = amountOfScalesToRemove;
        }

        public override string GetDescription()
        {
            return $"Remove {CurrentAmountOfScalesToRemove} scales";
        }

        public int PreviewAmount()
        {
            return CurrentAmountOfScalesToRemove;
        }
    }
}