using main.service.Turn_System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace main.view
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        
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
            gameService.OnGameOver.AddListener(Render);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        private void Render()
        {
            _container.SetActive(true);
        }
    }
}