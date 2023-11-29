using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Time Buff While On Board", menuName = "Data/Effects/Time Buff While On Board")]
    public class TimeBuffWhileOnBoardCE : CardEffect
    {
        [SerializeField] private int _amountOfTimeToAddEachTurn;
        private PlayerHandService playerHandService;

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        public override void Execute(int multiplier)
        {
            playerHandService.IncreaseTime(_amountOfTimeToAddEachTurn);
        }
    }
}