using main.service.Turn_System;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class CurrentTurnView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentTurnText;

        private TurnService turnService;
        
        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }

        private void OnEnable()
        {
            turnService.OnTurnNumberIncreased.AddListener(Render);
        }

        private void OnDisable()
        {
            turnService.OnTurnNumberIncreased.RemoveListener(Render);
        }

        private void Render(int currentTurnNumber)
        {
            _currentTurnText.text = currentTurnNumber.ToString();
        }
    }
}