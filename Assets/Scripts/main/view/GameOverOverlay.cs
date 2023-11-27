using main.service.Turn_System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace main.view
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _gameOverWinText;
        [SerializeField] private GameObject _gameOverLossText;
        
        private GameService gameService;
        
        [Inject]
        public void Construct(GameService gameService)
        {
            this.gameService = gameService;
        }

        private void Awake()
        {
            _container.SetActive(false);
        }

        private void Start()
        {
            gameService.OnGameOver.AddListener(() => Render(false));
            // TODO: OnGameWon -> Render(true)
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        private void Render(bool gameIsWon)
        {
            _gameOverWinText.SetActive(gameIsWon);
            _gameOverLossText.SetActive(!gameIsWon);
            _container.SetActive(true);
        }
    }
}