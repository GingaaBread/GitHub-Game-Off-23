using main.entity.Turn_System;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Remove Scales Per Time Remaining", menuName = "Data/Card Effect/Remove Scales Per Time Remaining")]

    public class RemoveScalesPerTimeRemainingCardEffect : RemoveScalesCardEffect
    {
        private Turn turn;

        [Inject]
        public void Construct(Turn turn)
        {
            this.turn = turn;
        }
        
        public override string GetDescription()
        {
            return $"Remove {currentAmountOfScalesToRemove} scales per time remaining after this card is played";
        }

        protected override int GetCurrentAmountOfScalesToRemove()
        {
            return currentAmountOfScalesToRemove * turn.RemainingTime.Time;
        }
    }
}