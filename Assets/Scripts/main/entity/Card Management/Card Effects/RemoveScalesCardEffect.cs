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

        private FishService fishService;

        public int PreviewAmount()
        {
            return amountOfScalesToRemove;
        }

        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        public override void Execute()
        {
            fishService.ScaleFish(amountOfScalesToRemove);
        }
    }
}