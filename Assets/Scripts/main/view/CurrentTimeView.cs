using main.entity.Turn_System;
using main.service.Turn_System;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class CurrentTimeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentTimeText;

        private TurnService turnService;
        private Turn turn;
        
        [Inject]
        public void Construct(TurnService turnService, Turn turn)
        {
            this.turnService = turnService;
            this.turn = turn;
        }

        private void OnEnable()
        {
            turnService.OnTurnRemainingTimeChanged += UpdateCurrentTime;
        }

        private void OnDisable()
        {
            turnService.OnTurnRemainingTimeChanged -= UpdateCurrentTime;
        }

        private void UpdateCurrentTime()
        {
            _currentTimeText.text = turn.RemainingTime.Time.ToString();
        }
    }
}