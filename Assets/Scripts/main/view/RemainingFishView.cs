using main.service.Turn_System;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class RemainingFishView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingFishText;
        private int _current;

        private GameService gameService;

        private void Start()
        {
            Render();
        }

        [Inject]
        public void Construct(GameService gameService)
        {
            this.gameService = gameService;
        }

        public void IncrementAndRender()
        {
            _current++;
            Render();
        }

        private void Render()
        {
            _remainingFishText.text = _current + "/" + gameService.RequiredAmountOfFishToScaleToWin();
        }
    }
}