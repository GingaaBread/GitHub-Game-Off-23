using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Draw Card", menuName = "Data/Card Effect/Draw Card")]
    public class DrawCardEffect : CardEffect
    {
        [SerializeField] private int amountOfCardsToDraw;

        private PlayerHandService playerHandService;
        
        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }
        
        public override void Execute()
        {
            playerHandService.Draw(amountOfCardsToDraw);
        }
    }
}