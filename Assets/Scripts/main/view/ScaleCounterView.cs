using main.service.Turn_System;
using main.service.Card_Management;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class ScaleCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scaleCounterText;
        
        private TurnService turnService;
        private PlayerHandService playerHandService;
        private int scaleCounter;
        
        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        private void Start()
        {
            turnService.OnNewTurnStart.AddListener(ClearScaleCounter);
            playerHandService.ScaleCounterShouldIncrease.AddListener(AddToScaleCounter);
            ClearScaleCounter();
        }

        private void Render(int amountToScale)
        {
            if(amountToScale == 0)gameObject.SetActive(false);
            else gameObject.SetActive(true);
            _scaleCounterText.text = amountToScale.ToString();
        }

        private void AddToScaleCounter(int amountToScale){
            scaleCounter += amountToScale;
            Render(scaleCounter);
        }

        private void ClearScaleCounter(){
            scaleCounter = 0;
            Render(0);
        }
    }
}