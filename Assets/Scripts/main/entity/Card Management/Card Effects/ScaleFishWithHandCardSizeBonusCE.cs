using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.service.Fish_Management;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    /// <summary>
    ///     A simple example of how classes can define card effects.
    ///     This simply defines a field "message" that will be printed upon execution.
    ///     NOTE: Can be removed once the actual card effects exist.
    /// </summary>
    [CreateAssetMenu(fileName = "Scale Fish With Hand Card Size Bonus", menuName = "Data/Effects/Scale Fish With Hand Card Size Bonus")]
    public class ScaleFishWithHandCardSizeBonusCE : CardEffect
    {
        [SerializeField] private int _baseAmountOfScales;
        [SerializeField] private int _bonusAmountForEachCardInHand;
        private FishService fishService;
        private PlayerHandService playerHandService;

        [Inject]
        public void Construct(FishService fishService){
            this.fishService = fishService;
        }

        [Inject]
        public void Construct(PlayerHandService playerHandService){
            this.playerHandService = playerHandService;
        }

        public override void Execute(int multiplier)
        {
            int scalesToRemove = AmountOfScalesToRemove() * multiplier;
            if(scalesToRemove > 0) fishService.ScaleFish(scalesToRemove);
        }

        public int AmountOfScalesToRemove(){
            int cardsRemaining = playerHandService.RemainingCards();
            return _bonusAmountForEachCardInHand * cardsRemaining;
        }
    }
}