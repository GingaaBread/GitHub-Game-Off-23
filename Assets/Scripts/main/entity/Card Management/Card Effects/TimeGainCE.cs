using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Time Gain", menuName = "Data/Effects/Time Gain")]
    public class TimeGainCE : CardEffect
    {
        [SerializeField] private int _amountOfTimeToGain;
        private PlayerHandService playerHandService;

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        public override void Execute(int multiplier)
        {
            Debug.Log("Increasing Time");
            playerHandService.IncreaseTime(_amountOfTimeToGain * multiplier);
        }
    }
}