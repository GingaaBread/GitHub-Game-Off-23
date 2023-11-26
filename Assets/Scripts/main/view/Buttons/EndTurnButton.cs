using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.view.Buttons
{
    public class EndTurnButton : MonoBehaviour
    {
        private TurnService turnService;

        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }
        
        public void OnClick()
        {
            turnService.EndTurn();
        }
    }
}