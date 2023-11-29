using main.entity.Card_Management.Card_Data;
using main.service.Fish_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Remove Scales", menuName = "Data/Effects/Remove Scales")]
    public class RemoveScalesCardEffect : CardEffect
    {
        [SerializeField] private int amountOfScalesToRemove;

        private FishService fishService;
        
        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }
        
        public override void Execute(int multiplier)
        {
            int scalesToRemove = AmountOfScalesToRemove() * multiplier;
            fishService.ScaleFish(scalesToRemove);
        }

        public int AmountOfScalesToRemove(){
            return amountOfScalesToRemove;
        }
    }
}