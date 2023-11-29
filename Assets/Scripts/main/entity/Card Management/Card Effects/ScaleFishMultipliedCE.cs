using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.service.Fish_Management;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Scale Fish Multiplied", menuName = "Data/Effects/Scale Fish Multiplied")]
    public class ScaleFishMultipliedCE : CardEffect
    {
        [SerializeField] private int _baseAmountOfScales;
        [SerializeField] private int _incrementAmountOfScales;
        [SerializeField] private bool _multiplyOnceForEachCardNamePlayed;
        [SerializeField] public Card playedCardRef;
        [SerializeField] private int _multiplicationLimit;
        private FishService fishService;
        private PlayerHandService playerHandService;
        
        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        public override void Execute(int multiplier)
        {
            int scalesToRemove = AmountOfScalesToRemove() * multiplier;
            if(scalesToRemove > 0)fishService.ScaleFish(scalesToRemove);
        }

        public int AmountOfScalesToRemove(){
            if(!_multiplyOnceForEachCardNamePlayed) return _baseAmountOfScales;
            int countedSoFar = 0;
            foreach(PlayedCardCounter playedCard in playerHandService.playedCardCounter){
                if (playedCard.CardName() == playedCardRef.Name){
                    countedSoFar = playedCard.CountedSoFar();
                    Debug.Log("Playing with " + countedSoFar + " counted");
                }
            }
            return AmountToRemovePerCount(countedSoFar);
        }

        public int EstimateAmountOfScalesToRemove(){
            if(!_multiplyOnceForEachCardNamePlayed) return _baseAmountOfScales;
            int countedSoFar = 0;
            foreach(PlayedCardCounter playedCard in playerHandService.playedCardCounter){
                if (playedCard.CardName() == playedCardRef.Name){
                    countedSoFar = playedCard.CurrentAmount();
                }
            }
            return AmountToRemovePerCount(countedSoFar);
        }

         public int AmountToRemovePerCount(int count){
            if(count < 1){
                return 0; //should not happen
            }
            else if (count == 1){
                return _baseAmountOfScales; //if only one of the card was played, there is no bonus
            }
            else{
                //total minus the amount from previous cards
                int previouslyPlayed = 1;
                int previouslyRemoved = 0;
                while (previouslyPlayed < count){
                    previouslyRemoved += AmountToRemovePerCount(previouslyPlayed);
                    previouslyPlayed++;
                }
                return (_baseAmountOfScales * count * _incrementAmountOfScales * Mathf.Clamp(count, 0, _multiplicationLimit)) - previouslyRemoved;
            }
        }
    }
}